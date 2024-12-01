using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    //서버 관련 변수
    public SecretGridServer secretGridServer = null;
    //점수 불러오기 관련 변수
    public GameObject RankPrefab;
    public GameObject ScollViewContent;
    int Testcount = 0;
    public GameObject targetItem;  // 스크롤할 대상 항목
    public GameObject EndItem;
    public ScrollRect scrollRect;

    public GameObject content;

    [SerializeField]
    private Color highlightColor;

    private IEnumerator Start()
    {
        yield return secretGridServer.WaitForReady(); // 서버가 준비될 때까지 기다린다.
        yield return secretGridServer.RequestLeaderboard("totalScore"); // 테스트 리더보드 정보 가져온다.
        RefreshRankInfo();
    }

    public void ClickRankBtn(string LeaderboardType)
    {
        StartCoroutine(ClickRankType(LeaderboardType));
    }

    public IEnumerator ClickRankType(string LeaderboardType)
    {
        yield return ResetRankInfo();
        string tmpstring = LeaderboardType;
        StartCoroutine(SetRankType(tmpstring));
    }

    public IEnumerator SetRankType(string LeaderboardType)
    {
        string tmpstring = LeaderboardType;

        yield return secretGridServer.WaitForReady(); // 서버가 준비될 때까지 기다린다.
        yield return secretGridServer.RequestLeaderboard(tmpstring); // 테스트 리더보드 정보 가져온다.
        RefreshRankInfo();
    }

    private void RefreshRankInfo()
    {
        if (!secretGridServer)
        {
            return;
        }

        if (secretGridServer.CachedLeaderboardResult == null)
        {
            return;
        }

        if (secretGridServer.CachedLeaderboardResult.entries == null)
        {
            return;
        }

        Testcount = 0;

        foreach (var entry in secretGridServer.CachedLeaderboardResult.entries)
        {
            GameObject myInstance = Instantiate(RankPrefab, ScollViewContent.transform);
            myInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (entry.rank + 1).ToString();
            myInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = entry.nickname;
            myInstance.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = entry.score.ToString(CultureInfo.InvariantCulture);

            myInstance.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = (entry.rank + 1).ToString();
            myInstance.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = entry.nickname;
            myInstance.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = entry.score.ToString(CultureInfo.InvariantCulture);

            Testcount++;

            // 나의 정보는 빨간색 표시
            if (entry.rank == secretGridServer.CachedLeaderboardResult.myRank)
            {
                myInstance.transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = highlightColor;
                myInstance.transform.GetChild(4).GetComponent<TextMeshProUGUI>().color = highlightColor;
                myInstance.transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = highlightColor;
                targetItem = myInstance;
            }

            if (Testcount == secretGridServer.CachedLeaderboardResult.entries.Count)
            {
                EndItem = myInstance;
                StartCoroutine(RefreshMyInfo());
            }
        }
    }

    private IEnumerator ResetRankInfo()
    {
        yield return null;
        Testcount = 0;
        targetItem = null;
        EndItem = null;
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

    }

    private IEnumerator RefreshMyInfo()
    {
        yield return EndItem;

        if (targetItem != null && scrollRect != null && EndItem != null)
        {
            if (targetItem.GetComponent<RectTransform>().transform.localPosition.y == -50 || targetItem.GetComponent<RectTransform>().transform.localPosition.y == -150)
            {
                scrollRect.verticalNormalizedPosition = 1;
            }
            else if (targetItem.GetComponent<RectTransform>().transform.localPosition.y - EndItem.GetComponent<RectTransform>().transform.localPosition.y > 500)
            {
                float targetNormalizedPosition = targetItem.GetComponent<RectTransform>().transform.localPosition.y - EndItem.GetComponent<RectTransform>().transform.localPosition.y;
                targetNormalizedPosition /= EndItem.GetComponent<RectTransform>().transform.localPosition.y;
                // 스크롤을 맨 위로 위치시키기 위해, targetNormalizedPosition을 사용
                scrollRect.verticalNormalizedPosition = 1 + targetNormalizedPosition;
            }
            else if (targetItem.GetComponent<RectTransform>().transform.localPosition.y - EndItem.GetComponent<RectTransform>().transform.localPosition.y <= 500)
            {
                scrollRect.verticalNormalizedPosition = 0;
            }
        }
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
