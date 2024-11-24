using System.Collections.Generic;
using UnityEngine;

public class HeroGameCaseUIData
{
    public List<HeroGameCaseDetailUIControlData> CaseDataList { get; private set; }

    public HeroGameCaseUIData(List<HeroGameCaseDetailUIControlData> caseDataList)
    {
        CaseDataList = caseDataList;
    }
}

public class HeroGameCaseUI : MonoBehaviour
{
    [SerializeField]
    private HeroGameCaseListUI caseListUI;

    [SerializeField]
    private HeroGameCaseDetailUIControl caseDetailUI;

    private HeroGameCaseUIData data;

    public void Apply(HeroGameCaseUIData data)
    {
        this.data = data;

        caseListUI.Apply(data);
    }

    public void ActiveCaseListUI()
    {
        caseListUI.Apply(data);
        caseListUI.gameObject.SetActive(true);

        caseDetailUI.gameObject.SetActive(false);
    }

    public void ActiveDetailUI(int caseIndex)
    {
        caseDetailUI.Apply(data.CaseDataList[caseIndex]);
        caseDetailUI.gameObject.SetActive(true);

        caseListUI.gameObject.SetActive(false);
    }
}
