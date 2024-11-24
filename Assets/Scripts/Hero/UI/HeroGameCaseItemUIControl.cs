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

        image.sprite = data.CaseStaticData.Sprite;
        titleText.text = data.CaseStaticData.Title_En;
    }

    private void OnClick()
    {
        HeroGameUI.I.ActiveCaseDetailUI(data.CaseIndex);
        AudioManager.I.PlaySFX(SFXType.Select);
    }
}
