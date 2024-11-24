using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroGameStatItemUIControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private HeroGameStatType statType;

    [SerializeField]
    private TMP_Text valueText;

    [SerializeField]
    private Vector2 tooltipOffset;

    [SerializeField]
    private float tooltipWidth = 160f;

    public void Apply(int value)
    {
        valueText.text = $"{statType.ToIconString()}{value}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HeroGameTooltipUI.I.Show(statType.ToTooltipString(), tooltipOffset, tooltipWidth);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HeroGameTooltipUI.I.Hide();
    }
}
