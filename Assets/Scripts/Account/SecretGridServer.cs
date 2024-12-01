using System;
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
    private int messageCounter = -1;

    private string lastResponseStr;

    // Awake 단계 초기화 완료됐는지?
    private bool IsInit { get; set; }

    // HTTP 네트워크가 진행 중인지?
    private bool IsNetworkingInProgress { get; set; }

    // Start() 시점의 초기화도 끝나서 이후 서버 작업을 진행할 수 있는 단계인지?
    public bool IsReady { get; private set; }

    // 로그인이 성공한 상태인지?
    public bool IsLoggedIn { get; private set; }

    private void Awake()
    {
        Init();
    }

    public IEnumerator WaitForReady()
    {
        var waitStart = Time.realtimeSinceStartup;
        const float timeout = 10.0f;
        while (IsReady == false && Time.realtimeSinceStartup - waitStart < timeout)
        {
            yield return null;
        }

        if (IsReady == false)
        {
            Debug.LogError("WaitForReady() Timeout!!!");
        }
    }

    private IEnumerator Start()
    {
        // 신규 가입 신청 매번 한다. 서버에 이미 있는 계정이면 401 반환된다.
        yield return RequestEnrollmentCoro();

        // 로그인 시작
        yield return RequestLoginCoro();

        IsReady = true;
    }

    private void Init()
    {
        if (IsInit)
        {
            return;
        }

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

        IsInit = true;
    }

    private IEnumerator RequestLoginCoro()
    {
        if (IsNetworkingInProgress)
        {
            Debug.LogWarning("Another networking request is in progress.");
            yield break;
        }
        IsNetworkingInProgress = true;
        yield return RequestLoginCoro_Internal();
        IsNetworkingInProgress = false;
    }

    private IEnumerator RequestLoginCoro_Internal()
    {
        if (IsLoggedIn)
        {
            yield break;
        }

        BigInteger a = Utils.GenerateRandomBigInteger(32); // 256비트 랜덤 수
        BigInteger A = BigInteger.ModPow(Parameters.g, a, Parameters.N);
        var A_bytes = A.ToByteArray(isUnsigned: true, isBigEndian: true);
        var A_hex = Utils.ToHex(A_bytes);

        ConDebug.Log($"A: {A_hex}");

        var formData = new List<IMultipartFormSection>();
        using var www = UnityWebRequest.Post($"{serverAddr}/login", formData);
        www.SetRequestHeader("X-User-Id", userId);
        www.SetRequestHeader("X-Public", A_hex);
        www.timeout = 10;
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
        www.timeout = 10;
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
                messageCounter = 0;
                
                IsLoggedIn = true;
                
                // 닉네임 정보 서버로 전달한다.
                yield return SendSecureMessageCoro_Internal($"SetNickname\t{nickname}");

                if (lastResponseStr != "OK")
                {
                    Debug.LogError($"Network protocol error 1");
                }
            }
        }

        if (serverLogText)
        {
            serverLogText.text += "\n" + (www.result == UnityWebRequest.Result.Success ? www.downloadHandler.text : www.error);
        }
    }

    public IEnumerator RequestLeaderboard(string stageId)
    {
        yield return SendSecureMessageCoro("GetLeaderboard", stageId);
        
        CachedLeaderboardResult = new LeaderboardResult();

        JsonUtility.FromJsonOverwrite(lastResponseStr, CachedLeaderboardResult);
    }

    private IEnumerator SendSecureMessageCoro(params string[] values)
    {
        yield return WaitForReady();
        
        if (IsNetworkingInProgress)
        {
            Debug.LogWarning("Another networking request is in progress.");
            yield break;
        }
        IsNetworkingInProgress = true;
        yield return SendSecureMessageCoro_Internal(string.Join('\t', values));
        IsNetworkingInProgress = false;
    }

    private IEnumerator SendSecureMessageCoro_Internal(string plaintext)
    {
        if (IsLoggedIn == false)
        {
            Debug.LogError($"Not connected to server. Is server '{serverAddr}' running? Sending the secure message '{plaintext}' failed.");
            yield break;
        }
        
        messageCounter++;

        var iv = new byte[16];
        RandomNumberGenerator.Fill(iv);
        ConDebug.Log($"IV: {Utils.ToHex(iv)}");

        var encrypted = AESUtil.Cipher.Encrypt(Encoding.UTF8.GetBytes($"{messageCounter}\t{plaintext}"), sharedK, iv);

        ConDebug.Log($"encrypted: {Convert.ToBase64String(encrypted)}");

        yield return SendCiphertextMessageCoro(encrypted, iv);
    }

    private IEnumerator SendCiphertextMessageCoro(byte[] encrypted, byte[] iv)
    {
        var encryptedB64 = Convert.ToBase64String(encrypted);
        var ivB64 = Convert.ToBase64String(iv);

        using var www = UnityWebRequest.Post($"{serverAddr}/message", $"{encryptedB64}&{ivB64}", "text/plain");
        www.SetRequestHeader("X-User-Id", userId);
        www.timeout = 10;
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

            var firstTabIndex = responseStr.IndexOf("\t", StringComparison.Ordinal);
            if (int.TryParse(responseStr[..firstTabIndex], out var replyCounter) && replyCounter == messageCounter)
            {
                lastResponseStr = responseStr[(firstTabIndex + 1)..];
            }
            else
            {
                Debug.LogError($"Server replied with wrong counter! Expecting {messageCounter} but {replyCounter} arrived.");
            }
        }
        else
        {
            Debug.LogWarning($"Response from server with {www.responseCode}");
            lastResponseStr = string.Empty;
        }
    }

    public void SetServerAddr(string text)
    {
        serverAddr = text;
    }

    [Serializable]
    public class LeaderboardResult
    {
        public int myRank; // 내 순위 (0이면 1등이란 뜻, -1이면 등록되지 않은 상태란 뜻)
        public string myUserId; // 내 유저 ID (GUID형식)
        public List<LeaderboardEntry> entries; // 내 순위 앞 7명, 뒤 7명 순위가 들어 있는 목록

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"MyRank: {myRank}, MyUserId: {myUserId}, Entries: {entries.Count}");
            foreach (var t in entries)
            {
                sb.AppendLine(t.ToString());
            }
            return sb.ToString();
        }
    }

    [Serializable]
    public class LeaderboardEntry
    {
        public long rank; // 0이면 1등이란 뜻
        public string userId; // 유저 ID (GUID 형식)
        public float score; // 점수
        public string nickname; // 닉네임

        public override string ToString()
        {
            return $"{rank}, {userId}, {score}, {nickname}";
        }
    }

    public IEnumerator RequestEnrollmentCoro()
    {
        var userRecord = Account.CreateAccount(userId, password);

        if (IsNetworkingInProgress)
        {
            Debug.LogWarning("Another networking request is in progress.");
            yield break;
        }
        IsNetworkingInProgress = true;
        yield return RequestEnrollmentCoro_Internal(userRecord);
        IsNetworkingInProgress = false;
    }

    private IEnumerator RequestEnrollmentCoro_Internal(Account.UserRecord userRecord)
    {
        var formData = new List<IMultipartFormSection>();
        using var www = UnityWebRequest.Post($"{serverAddr}/enroll", formData);
        www.SetRequestHeader("X-User-Id", userId);
        www.SetRequestHeader("X-Salt", Utils.ToHex(userRecord.Salt));
        www.SetRequestHeader("X-Verifier", Utils.ToHex(userRecord.Verifier.ToByteArray(isUnsigned: true, isBigEndian: true)));
        www.timeout = 10;
        yield return www.SendWebRequest();

        if (serverLogText)
        {
            serverLogText.text = www.result == UnityWebRequest.Result.Success ? www.downloadHandler.text : www.error;
        }
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

    public void StartSendScore(HeroGameContext gameContext)
    {
        var endingScore = HeroGameFormula.CalculateScore_End(gameContext.GameState == GameState.EndByEnding, gameContext.Day);
        var playTimeScore = HeroGameFormula.CalculateScore_PlayTime(gameContext.GetPlayTime());
        var statScore = HeroGameFormula.CalculateScore_Stat(gameContext.Player);
        var totalScore = gameContext.GetScore();
        
        ConDebug.Log("Starting SendSecureMessageCoro() coroutine inside StartSendScore()...");
        StartCoroutine(
            SendSecureMessageCoro(
                "SetScore",
                endingScore.ToString(),
                playTimeScore.ToString(),
                statScore.ToString(),
                totalScore.ToString()
            ));
    }
}
