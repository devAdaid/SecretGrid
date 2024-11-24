using UnityEngine;
using UnityEngine.EventSystems;

public class HeroGameCaseSelectionConfirmButtonUIControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private HeroGameButtonBase button;

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
        HeroGameTooltipUI.I.Show(new Vector2(-50, 50), "Temp");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HeroGameTooltipUI.I.Hide();
    }

    private void OnClick()
    {
        HeroGameContextHolder.I.SelectCaseSelection(data.CaseIndex, data.SelectionIndex);
    }
}
