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
    public GameObject EndItem;
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
            if(Testcount == userCount)
            {
                EndItem = myInstance;
            }
        }

    }
    public void RefreshMyInfo()
    {
        if(targetItem != null && scrollRect != null)
        {

            float targetNormalizedPosition = targetItem.GetComponent<RectTransform>().transform.localPosition.y +220 - EndItem.GetComponent<RectTransform>().transform.localPosition.y;
            targetNormalizedPosition /= EndItem.GetComponent<RectTransform>().transform.localPosition.y;

            // 스크롤을 맨 위로 위치시키기 위해, targetNormalizedPosition을 사용
            scrollRect.verticalNormalizedPosition = 1 + targetNormalizedPosition;

        }
    }
}
