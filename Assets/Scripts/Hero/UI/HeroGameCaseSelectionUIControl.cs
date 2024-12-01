using TMPro;
using UnityEngine;

public class HeroGameCaseSelectionUIControlData
{
    public int SuccessPercent { get; private set; }
    public HeroGameCaseSelection Selection { get; private set; }

    public HeroGameCaseSelectionUIControlData(int successPercent, HeroGameCaseSelection selection)
    {
        SuccessPercent = successPercent;
        Selection = selection;
    }
}

public class HeroGameCaseSelectionUIControl : MonoBehaviour
{
    [SerializeField]
    private TMP_Text descriptionText;

    [SerializeField]
    private TMP_Text rewardText;

    [SerializeField]
    private HeroGameCaseSelectionConfirmButtonUIControl confirmButton;

    private HeroGameCaseSelectionUIControlData data;

    public void Apply(HeroGameCaseSelectionUIControlData data)
    {
        this.data = data;

        descriptionText.text = CommonSingleton.I.IsKoreanLanguage ? data.Selection.StaticData.Description_Ko : data.Selection.StaticData.Description_En;
        rewardText.text = data.Selection.StatReward.ToUIString();

        confirmButton.Apply(data);
    }
}
