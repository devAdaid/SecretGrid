using TMPro;
using UnityEngine;

public class LeaderboardSubmitScore : MonoBehaviour
{
    [SerializeField]
    private string TotalRankType;
    [SerializeField]
    private string StatRankType;

    public int TotalScore;
    //    statScoreText.TypeText($"Stat: {HeroGameFormula.CalculateScore_Stat(gameContext.Player)}");
    //    totalScoreText.TypeText($"Total: {gameContext.GetScore()}");

    [SerializeField]
    SecretGridServer secretGridServer;


    [SerializeField]
    TextMeshProUGUI serverLogText;

    public static LeaderboardSubmitScore Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    public void OnSubmit()
    {
        //TotalRankType 점수를 서버로 보내준다.
        secretGridServer.SetServerAddr(TotalRankType);
        StartCoroutine(secretGridServer.SubmitScoreCoro(TotalRankType, TotalScore));

        //StatRankType 점수를 서버로 보내준다.
        secretGridServer.SetServerAddr(StatRankType);
        StartCoroutine(secretGridServer.SubmitScoreCoro(StatRankType, TotalScore));
    }
}
