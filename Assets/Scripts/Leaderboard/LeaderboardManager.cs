using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    //서버 관련 변수
    public SecretGridServer secretGridServer = null;
    public string[] leaderboardResult;
    public TextMeshProUGUI serverLogText;
    List<(int, string, string, string)> entriesArray = new List<(int, string, string, string)>();
    //점수 불러오기 관련 변수
    public GameObject RankPrefab;
    public GameObject ScollViewContent;
    public int userCount = 0;
    public string Myuserkey = "";
    int Testcount = 0;
    public GameObject targetItem;  // 스크롤할 대상 항목
    public GameObject EndItem;
    public ScrollRect scrollRect;
    public static LeaderboardManager Instance = null;

    private void Awake()
    {
        Instance = this;

    }

    private IEnumerator Start()
    {
        secretGridServer.SetServerLogText(serverLogText);
        yield return secretGridServer.GetLeaderboardResultCoro("teststage");
        userCount = secretGridServer.CachedLeaderboardResult.Entries.Count;
        Myuserkey = PlayerPrefs.GetString("UserId");
        secretGridServer.SetServerLogText(serverLogText);

        // 줄바꿈을 기준으로 문자열을 나누기
        string[] lines = serverLogText.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        // 각 줄을 처리하여 Dictionary에 저장
        for (int i = 0; i < lines.Length; i++)
        {
            string[] entryParts = lines[i].Split(',');

            // 각 항목을 배열에 저장
            if (entryParts.Length == 4)
            {
                string userId = entryParts[1].Trim();        // 두 번째 항목 (UserId)
                string score = entryParts[2].Trim(); // 세 번째 항목 (Score)
                string nickname = entryParts[3].Trim();      // 네 번째 항목 (Nickname)

                // 배열에 저장 (인덱스는 i로 설정)
                entriesArray.Add((int.Parse(entryParts[0].Trim()), userId, score, nickname));
            }
        }

        RefreshRankInfo();

    }

    public void RefreshRankInfo()
    {

        for (int i = 0; i < userCount; i++)
        {
            GameObject myInstance = Instantiate(RankPrefab, ScollViewContent.transform);
            myInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = entriesArray[i].Item1.ToString();
            myInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = entriesArray[i].Item4;
            myInstance.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = entriesArray[i].Item3;

            Testcount++;
            if (i == secretGridServer.CachedLeaderboardResult.MyRank)
            {
                myInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255); // 나의 정보는 빨간색 표시
                myInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
                myInstance.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
                targetItem = myInstance;
            }
            if (Testcount == userCount)
            {
                EndItem = myInstance;
                StartCoroutine(RefreshMyInfo());
            }
        }
    }
    public IEnumerator RefreshMyInfo()
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
                float targetNormalizedPosition = targetItem.GetComponent<RectTransform>().transform.localPosition.y + 220 - EndItem.GetComponent<RectTransform>().transform.localPosition.y;
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
}
