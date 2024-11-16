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
        var userIdPath = Path.Combine(Application.persistentDataPath, "UserId.txt");
        try
        {
            userId = File.ReadAllText(userIdPath);
        }
        catch (FileNotFoundException)
        {
            userId = Guid.NewGuid().ToString();
            File.WriteAllText(userIdPath, userId);
        }
    }

    public void SetServerAddr(string text)
    {
        serverAddr = text;
    }

    public IEnumerator SubmitScoreCoro(string stageId, int score)
    {
        var nicknameBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(nickname));
        
        var formData = new List<IMultipartFormSection>();
        using var www = UnityWebRequest.Post($"http://{serverAddr}/score", formData);
        www.SetRequestHeader("X-User-Id", userId);
        www.SetRequestHeader("X-Stage-Id", stageId);
        www.SetRequestHeader("X-Score", score.ToString());
        www.SetRequestHeader("X-User-Nickname", nicknameBase64);
        yield return www.SendWebRequest();

        if (!serverLogText) yield break;
        
        serverLogText.text = www.result != UnityWebRequest.Result.Success ? www.error : www.downloadHandler.text;
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
