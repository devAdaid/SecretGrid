using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using ConditionalDebug;
using SRPClient;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SecretGridServer : MonoSingleton<SecretGridServer>
{
    [SerializeField]
    private string serverAddr;

    [SerializeField]
    private TextMeshProUGUI serverLogText;

    private string userId;
    private string password;
    private string nickname;
    public LeaderboardResult CachedLeaderboardResult { get; private set; }
    private byte[] sharedK;

    private IEnumerator Start()
    {
        Init();

        var account = Account.CreateAccount(userId, password);
        
        // 신규 가입 신청 매번 한다. 서버에 이미 있는 계정이면 401 반환된다.
        yield return RequestEnrollment(account);

        //
        // 로그인 시작
        //
        yield return RequestLogin();
    }

    private void Init()
    {
        var serverSettings = Resources.LoadAll<SecretGridServerSettings>("Server/SecretGridServerSettings").SingleOrDefault();
        if (serverSettings != null)
        {
            serverAddr = serverSettings.ServerAddr;
        }
        else
        {
            // 이 메시지가 나왔다면 서버 관리자에게 파일을 어떻게 생성하면 되는지 안내를 받으면 됩니다!
            ConDebug.LogError($"Create SecretGridServerSettings asset in Assets/Resources/Server!");
        }

        nickname = PlayerPrefs.GetString("Nickname");
        if (nickname.Length == 0)
        {
            nickname = NicknameGenerator.I.Generate();
            PlayerPrefs.SetString("Nickname", nickname);
        }

        userId = PlayerPrefs.GetString("UserId");
        if (userId.Length == 0)
        {
            userId = Guid.NewGuid().ToString();
            PlayerPrefs.SetString("UserId", userId);
        }
        
        password = PlayerPrefs.GetString("Password");
        if (password.Length == 0)
        {
            password = Guid.NewGuid().ToString();
            PlayerPrefs.SetString("Password", password);
        }
        
        PlayerPrefs.Save();
    }

    private IEnumerator RequestLogin()
    {
        BigInteger a = Utils.GenerateRandomBigInteger(32); // 256비트 랜덤 수
        BigInteger A = BigInteger.ModPow(Parameters.g, a, Parameters.N);
        var A_bytes = A.ToByteArray(isUnsigned: true, isBigEndian: true);
        var A_hex = Utils.ToHex(A_bytes);
        
        ConDebug.Log($"A: {A_hex}");
        
        var formData = new List<IMultipartFormSection>();
        using var www = UnityWebRequest.Post($"{serverAddr}/login", formData);
        www.SetRequestHeader("X-User-Id", userId);
        www.SetRequestHeader("X-Public", A_hex);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            var tokens = www.downloadHandler.text.Split("\t");
            var salt = tokens[0];
            var serverPublic = tokens[1];
            
            Debug.Log($"ServerPublic: {serverPublic}");
            
            var B = new BigInteger(Parameters.StringToByteArray(serverPublic), isUnsigned: true, isBigEndian: true);
            
            ConDebug.Log($"User ID: {userId}");
            ConDebug.Log($"User Password: {password}");
            ConDebug.Log($"Salt: {salt}");
            ConDebug.Log($"Server Public: {serverPublic}");
            
            byte[] uHash = Utils.SHA256Hash(
                A.ToByteArray(isUnsigned: true, isBigEndian: true),
                B.ToByteArray(isUnsigned: true, isBigEndian: true)
            );
            BigInteger u = new BigInteger(uHash, isUnsigned: true, isBigEndian: true);

            if (u.IsZero)
            {
                ConDebug.LogError("u 값이 0입니다. 인증 실패.");
                yield break;
            }

            // 클라이언트: x 계산
            byte[] xHash = Utils.SHA256Hash(Utils.FromHex(salt), Encoding.UTF8.GetBytes(password));
            BigInteger x = new BigInteger(xHash, isUnsigned: true, isBigEndian: true);

            // 클라이언트: S 계산
            BigInteger kx = (Parameters.k * BigInteger.ModPow(Parameters.g, x, Parameters.N)) % Parameters.N;
            BigInteger diff = (B + Parameters.N - kx) % Parameters.N; // (B - k * g^x) mod N
            BigInteger exponent = (a + u * x) % (Parameters.N - 1); // a + u * x
            BigInteger S_client = BigInteger.ModPow(diff, exponent, Parameters.N);

            // 클라이언트: K 계산
            byte[] K_client = Utils.SHA256Hash(S_client.ToByteArray(isUnsigned: true, isBigEndian: true));

            // H(N) 계산
            byte[] H_N = Utils.SHA256Hash(Parameters.N.ToByteArray(isUnsigned: true, isBigEndian: true));

            // H(g) 계산
            byte[] H_g = Utils.SHA256Hash(Parameters.g.ToByteArray(isUnsigned: true, isBigEndian: true));

            // H(N) XOR H(g)
            byte[] H_N_xor_H_g = new byte[H_N.Length];
            for (int i = 0; i < H_N.Length; i++)
            {
                H_N_xor_H_g[i] = (byte)(H_N[i] ^ H_g[i]);
            }
            
            // 클라이언트: M1 계산
            byte[] M1 = Utils.SHA256Hash(
                H_N_xor_H_g,
                Utils.SHA256Hash(Encoding.UTF8.GetBytes(userId)),
                Utils.FromHex(salt),
                A.ToByteArray(isUnsigned: true, isBigEndian: true),
                B.ToByteArray(isUnsigned: true, isBigEndian: true),
                K_client
            );
            
            yield return SendClientSessionProof(M1, K_client, A);
        }

        if (serverLogText)
        {
            serverLogText.text += "\n" + (www.result == UnityWebRequest.Result.Success ? www.downloadHandler.text : www.error);
        }
    }
    
    // 바이트 배열 비교 함수
    static bool CompareByteArrays(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            return false;

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
                return false;
        }
        return true;
    }

    private IEnumerator SendClientSessionProof(byte[] clientSessionProof, byte[] K_client, BigInteger A)
    {
        var formData = new List<IMultipartFormSection>();
        using var www = UnityWebRequest.Post($"{serverAddr}/clientSessionProof", formData);
        www.SetRequestHeader("X-User-Id", userId);
        www.SetRequestHeader("X-Client-Session-Proof", Utils.ToHex(clientSessionProof));
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            var serverProof = www.downloadHandler.text; // M2
            var M2 = Parameters.StringToByteArray(serverProof);
            
            byte[] M2_client = Utils.SHA256Hash(
                A.ToByteArray(isUnsigned: true, isBigEndian: true),
                clientSessionProof,
                K_client
            );

            if (!CompareByteArrays(M2, M2_client))
            {
                ConDebug.LogError("클라이언트에서 M2 검증 실패. 인증 거부.");
            }
            else
            {
                ConDebug.Log("Login successful!");
                ConDebug.Log($"K_client: {Utils.ToHex(K_client)}");
                sharedK = K_client;
                
                var iv = new byte[16];
                RandomNumberGenerator.Fill(iv);
                ConDebug.Log($"IV: {Utils.ToHex(iv)}");
                const int messageCounter = 1;
                var encrypted = AESUtil.Cipher.Encrypt(Encoding.UTF8.GetBytes($"{messageCounter}\t암호화된 세션으로 처음 보내는 메시지! 게임잼 재미있다"), sharedK, iv);
                
                ConDebug.Log($"encrypted: {Convert.ToBase64String(encrypted)}");

                yield return SendSecureMessage(encrypted, iv);
            }
        }

        if (serverLogText)
        {
            serverLogText.text += "\n" + (www.result == UnityWebRequest.Result.Success ? www.downloadHandler.text : www.error);
        }
    }

    private IEnumerator SendSecureMessage(byte[] encrypted, byte[] iv)
    {
        var encryptedB64 = Convert.ToBase64String(encrypted);
        var ivB64 = Convert.ToBase64String(iv);
        
        using var www = UnityWebRequest.Post($"{serverAddr}/message", $"{encryptedB64}&{ivB64}", "text/plain");
        www.SetRequestHeader("X-User-Id", userId);
        yield return www.SendWebRequest();

        if (www.responseCode == (long)HttpStatusCode.OK)
        {
            var response = www.downloadHandler.text;
            Debug.Log($"Response from server (ciphertext): {response}");

            var responseTokens = response.Split("&");
            var responseCiphertextB64 = responseTokens[0];
            var responseIVB64 = responseTokens[1];

            var responseCiphertext = Convert.FromBase64String(responseCiphertextB64);
            var responseIV = Convert.FromBase64String(responseIVB64);
            var responsePlaintext = AESUtil.Cipher.Decrypt(responseCiphertext, sharedK, responseIV);
            var responseStr = Encoding.UTF8.GetString(responsePlaintext);
            Debug.Log($"Response from server (plaintext): {responseStr}");
        }
        else
        {
            Debug.LogWarning($"Response from server with {www.responseCode}");
        }
    }

    public void SetServerAddr(string text)
    {
        serverAddr = text;
    }

    public class LeaderboardResult
    {
        public int MyRank; // 내 순위 (0이면 1등이란 뜻, -1이면 등록되지 않은 상태란 뜻)
        public string MyUserId; // 내 유저 ID (GUID형식)
        public List<LeaderboardEntry> Entries; // 내 순위 앞 7명, 뒤 7명 순위가 들어 있는 목록

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"MyRank: {MyRank}, MyUserId: {MyUserId}, Entries: {Entries.Count}");
            foreach (var t in Entries)
            {
                sb.AppendLine(t.ToString());
            }
            return sb.ToString();
        }
    }

    public class LeaderboardEntry
    {
        public int Rank; // 0이면 1등이란 뜻
        public string UserId; // 유저 ID (GUID 형식)
        public float Score; // 점수
        public string Nickname; // 닉네임

        public override string ToString()
        {
            return $"{Rank}, {UserId}, {Score}, {Nickname}";
        }
    }

    private IEnumerator RequestEnrollment(Account.UserRecord userRecord)
    {
        var formData = new List<IMultipartFormSection>();
        using var www = UnityWebRequest.Post($"{serverAddr}/enroll", formData);
        www.SetRequestHeader("X-User-Id", userId);
        www.SetRequestHeader("X-Salt", Utils.ToHex(userRecord.Salt));
        www.SetRequestHeader("X-Verifier", Utils.ToHex(userRecord.Verifier.ToByteArray(isUnsigned: true, isBigEndian: true)));
        yield return www.SendWebRequest();

        if (serverLogText)
        {
            serverLogText.text = www.result == UnityWebRequest.Result.Success ? www.downloadHandler.text : www.error;
        }
    }

    public IEnumerator SubmitScoreCoro(string stageId, int score)
    {
        var nicknameBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(nickname));

        var formData = new List<IMultipartFormSection>();
        using var www = UnityWebRequest.Post($"{serverAddr}/score", formData);
        www.SetRequestHeader("X-User-Id", userId);
        www.SetRequestHeader("X-Stage-Id", stageId);
        www.SetRequestHeader("X-Score", score.ToString());
        www.SetRequestHeader("X-User-Nickname", nicknameBase64);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            CachedLeaderboardResult = ParseResult(www.downloadHandler.text);
        }
        else
        {
            ConDebug.LogError($"SubmitScoreCoro: Failed to parse result: {www.error} ({www.url})");
        }

        if (serverLogText)
        {
            serverLogText.text = CachedLeaderboardResult != null ? CachedLeaderboardResult.ToString() : www.error;
        }
    }

    public IEnumerator GetLeaderboardResultCoro(string stageId)
    {
        Init();
            
        yield return SubmitScoreCoro(stageId, -1);
    }

    private static LeaderboardResult ParseResult(string resultText)
    {
        LeaderboardResult result = new LeaderboardResult {
            MyRank = -1,
            Entries = new List<LeaderboardEntry>(),
        };

        var resultTokens = resultText.Split("\t");
        var tokenCounter = 0;
        if (resultTokens.Length > 0)
        {
            result.MyRank = int.Parse(resultTokens[tokenCounter]);
            tokenCounter++;
            result.MyUserId = resultTokens[tokenCounter];
            tokenCounter++;
        }

        int myRankIndex = -1;

        while (tokenCounter + 3 <= resultTokens.Length)
        {
            var leaderboardEntry = new LeaderboardEntry {
                UserId = resultTokens[tokenCounter],
                Score = float.Parse(resultTokens[tokenCounter + 1]),
                Nickname = resultTokens[tokenCounter + 2],
            };

            if (leaderboardEntry.UserId == result.MyUserId)
            {
                myRankIndex = result.Entries.Count;
            }

            result.Entries.Add(leaderboardEntry);

            tokenCounter += 3;
        }

        for (var index = 0; index < result.Entries.Count; index++)
        {
            var entry = result.Entries[index];

            entry.Rank = index - myRankIndex + result.MyRank;
        }

        return result;
    }

    public void SetServerLogText(TextMeshProUGUI inServerLogText)
    {
        serverLogText = inServerLogText;
    }

    public void SetNickname(string inNickname)
    {
        nickname = inNickname;
    }

    [Button("Test Button")]
    private void TestButton()
    {
        ConDebug.Log("Test Button clicked!");
    }
}
