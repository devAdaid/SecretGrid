using TMPro;
using UnityEngine;

public class HeroGameCaseSelectionUIControlData
{
    public int CaseIndex { get; private set; }
    public int SelectionIndex { get; private set; }
    public int SuccessPercent { get; private set; }
    public HeroGameCaseSelectionStaticData SelectionStaticData { get; private set; }

    public HeroGameCaseSelectionUIControlData(int caseIndex, int selectionIndex, int successPercent, HeroGameCaseSelectionStaticData selectionStaticData)
    {
        CaseIndex = caseIndex;
        SelectionIndex = selectionIndex;
        SuccessPercent = successPercent;
        SelectionStaticData = selectionStaticData;
    }
}

public class HeroGameCaseSelectionUIControl : MonoBehaviour
{
    [SerializeField]
    private TMP_Text descriptionText;

    [SerializeField]
    private HeroGameCaseSelectionConfirmButtonUIControl confirmButton;

    private HeroGameCaseSelectionUIControlData data;

    public void Apply(HeroGameCaseSelectionUIControlData data)
    {
        this.data = data;

        descriptionText.text = data.SelectionStaticData.Description;
        confirmButton.Apply(data);
    }
}
