using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SecretGridServer : MonoBehaviour
{
    [SerializeField]
    private string serverAddr;
    
    private TextMeshProUGUI serverLogText;
    
    private string userId;
    private string nickname;

    private void Awake()
    {
        userId = PlayerPrefs.GetString("UserId");
        if (userId.Length != 0)
        {
            return;
        }
        
        userId = Guid.NewGuid().ToString();
        PlayerPrefs.SetString("UserId", userId);
        PlayerPrefs.Save();
    }

    public void SetServerAddr(string text)
    {
        serverAddr = text;
    }

    private class LeaderboardResult
    {
        public int MyRank;
        public string MyUserId;
        public List<LeaderboardEntry> Entries;

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

    private class LeaderboardEntry
    {
        public string UserId;
        public float Score;
        public string Nickname;

        public override string ToString()
        {
            return $"{UserId}, {Score}, {Nickname}";
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

        if (!serverLogText) yield break;

        var leaderboardResult = ParseResult(www.result != UnityWebRequest.Result.Success ? www.error : www.downloadHandler.text);
        
        serverLogText.text = leaderboardResult.ToString();
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

        while (tokenCounter + 3 <= resultTokens.Length)
        {
            var leaderboardEntry = new LeaderboardEntry {
                UserId = resultTokens[tokenCounter],
                Score = float.Parse(resultTokens[tokenCounter + 1]),
                Nickname = resultTokens[tokenCounter + 2],
            };
            
            result.Entries.Add(leaderboardEntry);

            tokenCounter += 3;
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
