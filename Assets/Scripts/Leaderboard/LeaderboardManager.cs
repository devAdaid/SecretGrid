using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{   

    public GameObject RankPrefab;
    public GameObject ScollViewContent;

    public int userCount = 20;
    public string Myuserkey = "Nick";

    int Testcount = 0;

    public GameObject targetItem;  // 스크롤할 대상 항목
    public ScrollRect scrollRect;
    private int UserCount
    {
        get { return userCount; }
        //set { userCount =  Count; } => 서버로 부터 받아오는 인원수
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshRankInfo();
    }


    public void RefreshRankInfo()
    {

        for(int i=0; i< userCount; i++)
        {
            GameObject myInstance = Instantiate(RankPrefab, ScollViewContent.transform);
            //myInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Rank.ToString(); => 서버로 부터 받아오는 랭킹
            //myInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Nick; => 서버로 부터 받아오는 닉네임
            //myInstance.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Score.ToString(); => => 서버로 부터 받아오는 점수
            Testcount++;
            myInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Testcount.ToString();
            if (Testcount == 10)
            {
                myInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Nick";
            }
            if (Myuserkey == myInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text)
            {
                myInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255); // 나의 정보는 빨간색 표시
                myInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
                myInstance.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
                targetItem = myInstance;
            }
        }

        RefreshMyInfo();
    }
    public void RefreshMyInfo()
    {
        if(targetItem != null && scrollRect != null)
        {
            // 대상 항목의 RectTransform을 가져옵니다
            RectTransform targetRectTransform = targetItem.GetComponent<RectTransform>();

            // ScrollView 내에서 대상 항목의 위치를 계산합니다
            Vector3[] worldCorners = new Vector3[4];
            targetRectTransform.GetWorldCorners(worldCorners);

            // ScrollView의 월드 위치를 가져옵니다
            RectTransform scrollRectTransform = scrollRect.GetComponent<RectTransform>();
            Vector3[] scrollCorners = new Vector3[4];
            scrollRectTransform.GetWorldCorners(scrollCorners);

            // ScrollView의 영역 내에서 항목의 상대적인 위치 계산
            float itemPositionY = worldCorners[0].y - scrollCorners[0].y;
            float scrollHeight = scrollRectTransform.rect.height;

            // 항목의 높이를 계산합니다 (Y 좌표 차이를 사용)
            float itemHeight = worldCorners[2].y - worldCorners[0].y;

            // 스크롤 위치를 설정 (0 - 1 사이로)
            float targetNormalizedPosition = itemPositionY / scrollHeight;

            // 한 항목 아래로 스크롤 이동
            float newNormalizedPosition = targetNormalizedPosition - (itemHeight / scrollHeight);

            Debug.Log(newNormalizedPosition);
            // 유효한 범위로 제한 (0 ~ 1 사이)
            newNormalizedPosition = Mathf.Clamp01(newNormalizedPosition);

            // 스크롤을 한 항목 아래로 이동
            scrollRect.verticalNormalizedPosition = 1 - newNormalizedPosition;
        }
    }
}
