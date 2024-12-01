using System;
using System.Collections;
using System.Globalization;
using ConditionalDebug;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    //점수 불러오기 관련 변수
    [SerializeField]
    [FormerlySerializedAs("RankPrefab")]
    private GameObject rankPrefab;
    
    [SerializeField]
    [FormerlySerializedAs("scollViewContent")]
    [FormerlySerializedAs("ScollViewContent")]
    private GameObject scrollViewContent;
    
    [SerializeField]
    private ScrollRect scrollRect;
    
    [SerializeField]
    private Color highlightColor;

    private IEnumerator Start()
    {
        yield return SetRankType("totalScore");
    }

    public void ClickRankBtn(string leaderboardType)
    {
        StartCoroutine(SetRankType(leaderboardType));
    }

    private IEnumerator SetRankType(string leaderboardType)
    {
        yield return SecretGridServer.I.WaitForReady();
        yield return SecretGridServer.I.RequestLeaderboard(leaderboardType);
        yield return RefreshRankInfo();
    }

    private void Update()
    {
        //Debug.Log(scrollRect.verticalNormalizedPosition);
    }

    private IEnumerator RefreshRankInfo()
    {
        if (SecretGridServer.I.CachedLeaderboardResult == null)
        {
            yield break;
        }

        if (SecretGridServer.I.CachedLeaderboardResult.entries == null)
        {
            yield break;
        }
        
        DestroyAllEntries();

        var myEntryIndex = -1;
        var entryHeight = 0.0f;

        for (var index = 0; index < SecretGridServer.I.CachedLeaderboardResult.entries.Count; index++)
        {
            var entry = SecretGridServer.I.CachedLeaderboardResult.entries[index];
            var myInstance = Instantiate(rankPrefab, scrollViewContent.transform);

            entryHeight = myInstance.GetComponent<RectTransform>().rect.height;
            
            myInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (entry.rank + 1).ToString();
            myInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = entry.nickname;
            myInstance.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = entry.score.ToString(CultureInfo.InvariantCulture);

            myInstance.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = (entry.rank + 1).ToString();
            myInstance.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = entry.nickname;
            myInstance.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = entry.score.ToString(CultureInfo.InvariantCulture);

            // 나의 정보는 빨간색 표시
            if (entry.rank == SecretGridServer.I.CachedLeaderboardResult.myRank)
            {
                myInstance.transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = highlightColor;
                myInstance.transform.GetChild(4).GetComponent<TextMeshProUGUI>().color = highlightColor;
                myInstance.transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = highlightColor;
                myEntryIndex = index;
            }
        }

        // 내 항목이 가운데 나오도록 스크롤 위치 조절
        if (myEntryIndex != -1 && SecretGridServer.I.CachedLeaderboardResult.entries.Count > 1)
        {
            yield return new WaitForEndOfFrame();
            
            var h = scrollRect.GetComponent<RectTransform>().rect.height;
            var H = scrollRect.content.rect.height;

            var x = entryHeight / 2 + myEntryIndex * entryHeight;
            var y = -1.0f / (H - h) * x + (H - h / 2) / (H - h);

            scrollRect.verticalNormalizedPosition = y;
        }
    }

    private void DestroyAllEntries()
    {
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
