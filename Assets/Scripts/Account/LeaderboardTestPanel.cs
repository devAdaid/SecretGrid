using TMPro;
using UnityEngine;

public class LeaderboardTestPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField serverAddrInputField;
    [SerializeField]
    private TMP_InputField stageIdInputField;
    [SerializeField]
    private TMP_InputField scoreInputField;
    [SerializeField]
    private TextMeshProUGUI nicknameText;
    
    [SerializeField]
    SecretGridServer secretGridServer;
    
    [SerializeField]
    TextMeshProUGUI serverLogText;

    private void Awake()
    {
        serverAddrInputField.text = PlayerPrefs.GetString("LastServerAddr", "http://localhost:24110");
    }

    public void OnSubmit()
    {
        secretGridServer.SetServerAddr(serverAddrInputField.text);
        secretGridServer.SetServerLogText(serverLogText);
        secretGridServer.SetNickname(nicknameText.text);
        serverLogText.text = "Wait for result...";
        StartCoroutine(secretGridServer.SubmitScoreCoro(stageIdInputField.text, int.Parse(scoreInputField.text)));
        
        PlayerPrefs.SetString("LastServerAddr", serverAddrInputField.text);
        PlayerPrefs.Save();
    }
}
