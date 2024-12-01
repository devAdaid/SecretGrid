using UnityEngine;
using UnityEngine.EventSystems;

public class SecretButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField]
    private float tooltipWidth = 50f;

    [SerializeField]
    private Vector2 tooltipOffset;

    public void OnPointerClick(PointerEventData eventData)
    {
        CommonSingleton.I.PersistentContext.SetSecret2Enable(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        HeroGameUI.I.TooltipUI.Show("?", tooltipOffset, tooltipWidth);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        HeroGameUI.I.TooltipUI.Hide();
    }
}
