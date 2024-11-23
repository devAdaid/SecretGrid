using UnityEngine;
using UnityEngine.EventSystems;

public class HeroGameCaseSelectionConfirmButtonUIControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private HintControl hintControl;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hintControl.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hintControl.gameObject.SetActive(false);
    }
}
