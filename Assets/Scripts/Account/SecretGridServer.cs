using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SecureRemotePassword;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SecretGridServer : MonoSingleton<SecretGridServer>
{
    [SerializeField]
    private string serverAddr;

    [SerializeField]
    private TextMeshProUGUI serverLogText;

    private string srpSalt;
    private string userId;
    private string password;
    private string nickname;
    public LeaderboardResult CachedLeaderboardResult { get; private set; }

    private IEnumerator Start()
    {
        Init();

        // SRP 프로토콜 시작
        var client = new SrpClient(SrpParameters.Create4096<SHA256>());
        srpSalt = PlayerPrefs.GetString("SrpSalt");
        if (srpSalt.Length == 0)
        {
            srpSalt = client.GenerateSalt();
            PlayerPrefs.SetString("SrpSalt", srpSalt);
        }
        PlayerPrefs.Save();
        
        var privateKey = client.DerivePrivateKey(srpSalt, userId, password);
        var verifier = client.DeriveVerifier(privateKey);
        
        Debug.Log($"User ID: {userId}");
        Debug.Log($"Password: {password}");
        Debug.Log($"Verifier: {verifier}");
        
        
        // 신규 가입 신청 매번 한다. 서버에 이미 있는 계정이면 401 반환된다.
        yield return RequestEnrollment(verifier);

        //
        // 로그인 시작
        //
        yield return RequestLogin(client);
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
            Debug.LogError($"Create SecretGridServerSettings asset in Assets/Resources/Server!");
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

    private IEnumerator RequestLogin(SrpClient client)
    {
        var clientEphemeral = client.GenerateEphemeral();
        
        var formData = new List<IMultipartFormSection>();
        using var www = UnityWebRequest.Post($"{serverAddr}/login", formData);
        www.SetRequestHeader("X-User-Id", userId);
        www.SetRequestHeader("X-Public", clientEphemeral.Public);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            var tokens = www.downloadHandler.text.Split("\t");
            var salt = tokens[0];
            var serverPublic = tokens[1];
            
            Debug.Log($"User ID: {userId}");
            Debug.Log($"User Password: {password}");
            Debug.Log($"Salt: {salt}");
            Debug.Log($"Server Public: {serverPublic}");
            
            var privateKey = client.DerivePrivateKey(salt, userId, password);
            var clientSession = client.DeriveSession(clientEphemeral.Secret, serverPublic, salt, userId, privateKey);
            
            yield return SendClientSessionProof(clientSession.Proof);
        }

        if (serverLogText)
        {
            serverLogText.text += "\n" + (www.result == UnityWebRequest.Result.Success ? www.downloadHandler.text : www.error);
        }
    }

    private IEnumerator SendClientSessionProof(string clientSessionProof)
    {
        var formData = new List<IMultipartFormSection>();
        using var www = UnityWebRequest.Post($"{serverAddr}/clientSessionProof", formData);
        www.SetRequestHeader("X-User-Id", userId);
        www.SetRequestHeader("X-Client-Session-Proof", clientSessionProof);
        yield return www.SendWebRequest();
        
        if (serverLogText)
        {
            serverLogText.text += "\n" + (www.result == UnityWebRequest.Result.Success ? www.downloadHandler.text : www.error);
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

    private IEnumerator RequestEnrollment(string verifier)
    {
        var formData = new List<IMultipartFormSection>();
        using var www = UnityWebRequest.Post($"{serverAddr}/enroll", formData);
        www.SetRequestHeader("X-User-Id", userId);
        www.SetRequestHeader("X-Salt", srpSalt);
        www.SetRequestHeader("X-Verifier", verifier);
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
            Debug.LogError($"SubmitScoreCoro: Failed to parse result: {www.error} ({www.url})");
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
        Debug.Log("Test Button clicked!");
    }
}
