using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroGameCaseDetailUIControlData
{
    public int CaseIndex { get; private set; }
    public HeroGameCaseStaticData CaseStaticData { get; private set; }
    public List<HeroGameCaseSelectionUIControlData> SelectionDataList { get; private set; }

    public HeroGameCaseDetailUIControlData(int caseIndex, HeroGameCaseStaticData caseStaticData, List<HeroGameCaseSelectionUIControlData> selectionDataList)
    {
        CaseIndex = caseIndex;
        CaseStaticData = caseStaticData;
        SelectionDataList = selectionDataList;
    }
}

public class HeroGameCaseDetailUIControl : MonoBehaviour
{
    [SerializeField]
    private Button backButton;

    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private TMP_Text descriptionText;

    [SerializeField]
    private RectTransform selectionControlRoot;

    [SerializeField]
    private List<HeroGameCaseSelectionUIControl> selectionControls = new List<HeroGameCaseSelectionUIControl>();

    public int CaseIndex { get; private set; }

    private void Awake()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    public void Apply(HeroGameCaseDetailUIControlData data)
    {
        CaseIndex = data.CaseIndex;

        titleText.text = data.CaseStaticData.Title;
        descriptionText.text = data.CaseStaticData.Description;

        for (var i = 0; i < data.SelectionDataList.Count; i++)
        {
            selectionControls[i].gameObject.SetActive(true);
            selectionControls[i].Apply(data.SelectionDataList[i]);
        }

        for (var i = data.SelectionDataList.Count; i < selectionControls.Count; i++)
        {
            selectionControls[i].gameObject.SetActive(false);
        }
    }

    public void OnBackButtonClicked()
    {
        HeroGameUI.I.ActiveCaseListUI();
    }
}
