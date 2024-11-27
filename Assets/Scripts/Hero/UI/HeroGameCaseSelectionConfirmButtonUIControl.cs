using UnityEngine;
using UnityEngine.EventSystems;

public class HeroGameCaseSelectionConfirmButtonUIControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private HeroGameButtonBase button;

    [SerializeField]
    private Vector2 tooltipOffset = new Vector2(-50, 50);

    [SerializeField]
    private float tooltipWidth = 80f;

    private HeroGameCaseSelectionUIControlData data;

    private void Awake()
    {
        button.AddOnClickListener(OnClick);
    }

    public void Apply(HeroGameCaseSelectionUIControlData data)
    {
        this.data = data;

        button.SetButtonText($"Success\n{data.SuccessPercent}%");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var tooltipText = data.Selection.GetTooltipText(data.SuccessPercent);
        if (string.IsNullOrEmpty(tooltipText))
        {
            return;
        }

        HeroGameTooltipUI.I.Show(
            tooltipText,
            tooltipOffset,
            tooltipWidth
        );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HeroGameTooltipUI.I.Hide();
    }

    private void OnClick()
    {
        //HeroGameContextHolder.I.SelectCaseSelection(data.Selection.CaseIndex, data.Selection.SelectionIndex);
        HeroGameUI.I.ShowCaseResultUI(data);
        HeroGameTooltipUI.I.Hide();
    }
}
