using System.Collections;
using TMPro;
using UnityEngine;

public class LeaderboardSubmitScore : MonoBehaviour
{
    [SerializeField]
    private string TotalRankType;
    [SerializeField]
    private string StatRankType;

    public int TotalScore;
    public int StatScore;

    [SerializeField]
    SecretGridServer secretGridServer;

    public static LeaderboardSubmitScore Instance;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator OnSubmit()
    {
        yield return secretGridServer.WaitForReady(); // 서버가 준비될 때까지 기다린다.

        //TotalRankType 점수를 서버로 보내준다.
        StartCoroutine(secretGridServer.SubmitScoreCoro(TotalRankType,TotalScore));

        //StatRankType 점수를 서버로 보내준다.
        StartCoroutine(secretGridServer.SubmitScoreCoro(StatRankType,StatScore));
    }
}
