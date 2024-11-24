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
    private Button leftButton;

    [SerializeField]
    private Button rightButton;

    [SerializeField]
    private Image caseImage;

    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private TMP_Text descriptionText;

    [SerializeField]
    private RectTransform selectionControlRoot;

    [SerializeField]
    private List<HeroGameCaseSelectionUIControl> selectionControls = new List<HeroGameCaseSelectionUIControl>();

    public int CaseIndex { get; private set; }

    private HeroGameCaseDetailUIControlData data;

    private void Awake()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
        leftButton.onClick.AddListener(OnLeftButtonClicked);
        rightButton.onClick.AddListener(OnRightButtonClicked);
    }

    public void Apply(HeroGameCaseDetailUIControlData data)
    {
        this.data = data;

        CaseIndex = data.CaseIndex;

        caseImage.sprite = data.CaseStaticData.Sprite;
        titleText.text = data.CaseStaticData.Title_En;
        descriptionText.text = data.CaseStaticData.Description_En;

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
        AudioManager.I.PlaySFX(SFXType.Cancel);
    }

    public void OnLeftButtonClicked()
    {
        var caseIndex = CaseIndex - 1;
        if (caseIndex < 0)
        {
            caseIndex = HeroGameContextHolder.I.GameContext.CurrentCases.Count - 1;
        }

        HeroGameUI.I.ActiveCaseDetailUI(caseIndex);
        AudioManager.I.PlaySFX(SFXType.Select);
    }

    public void OnRightButtonClicked()
    {
        var caseIndex = CaseIndex + 1;
        if (caseIndex >= HeroGameContextHolder.I.GameContext.CurrentCases.Count)
        {
            caseIndex = 0;
        }

        HeroGameUI.I.ActiveCaseDetailUI(caseIndex);
        AudioManager.I.PlaySFX(SFXType.Select);
    }
}
