using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecureRemotePassword;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SecretGridServer : MonoSingleton<SecretGridServer>
{
    [SerializeField]
    private string serverAddr;

    private TextMeshProUGUI serverLogText;

    private string srpSalt;
    private string userId;
    private string password;
    private string nickname;
    public LeaderboardResult CachedLeaderboardResult { get; private set; }

    private IEnumerator Start()
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

        // SRP 프로토콜 시작
        var client = new SrpClient();
        srpSalt = PlayerPrefs.GetString("SrpSalt");
        if (srpSalt.Length == 0)
        {
            srpSalt = client.GenerateSalt();
            PlayerPrefs.SetString("SrpSalt", srpSalt);
        }

        password = PlayerPrefs.GetString("Password");
        if (password.Length == 0)
        {
            password = Guid.NewGuid().ToString();
            PlayerPrefs.SetString("Password", password);
        }
        
        var privateKey = client.DerivePrivateKey(srpSalt, userId, password);
        var verifier = client.DeriveVerifier(privateKey);
        
        Debug.Log($"Verifier: {verifier}");

        PlayerPrefs.Save();
        
        yield return RequestEnrollment(verifier);
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

        serverLogText.text = www.result == UnityWebRequest.Result.Success ? www.downloadHandler.text : www.error;
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

        CachedLeaderboardResult = www.result == UnityWebRequest.Result.Success ? ParseResult(www.downloadHandler.text) : null;

        if (serverLogText)
        {
            serverLogText.text = CachedLeaderboardResult != null ? CachedLeaderboardResult.ToString() : www.error;
        }
    }

    public IEnumerator GetLeaderboardResultCoro(string stageId)
    {
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
}
