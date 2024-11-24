using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroGameCaseItemUIControl : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private HeroGameButtonBase button;

    private HeroGameCaseDetailUIControlData data;

    private void Awake()
    {
        button.AddOnClickListener(OnClick);
    }

    public void Apply(HeroGameCaseDetailUIControlData data)
    {
        this.data = data;

        titleText.text = data.CaseStaticData.Title;
    }

    private void OnClick()
    {
        HeroGameUI.I.ActiveCaseDetailUI(data.CaseIndex);
    }
}
