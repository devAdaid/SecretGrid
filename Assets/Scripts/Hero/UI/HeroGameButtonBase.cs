using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroGameButtonBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private TMP_Text buttonText;

    [SerializeField]
    private TMP_Text buttonShadowText;

    [SerializeField]
    [TextArea]
    private string buttonInitialText;

    [SerializeField]
    private float buttonYOffsetOnPressed = 0.1f;

    private Vector3 textLocalPosition;
    private Vector3 textShadowLocalPosition;

    private void Awake()
    {
        textLocalPosition = buttonText.transform.localPosition;
        textShadowLocalPosition = buttonShadowText.transform.localPosition;
    }

    private void OnValidate()
    {
        buttonText.text = buttonInitialText;
        buttonShadowText.text = buttonInitialText;
    }

    public void AddOnClickListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }

    public void SetButtonText(string text)
    {
        buttonText.text = text;
        buttonShadowText.text = text;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonText.transform.localPosition = textLocalPosition + new Vector3(0f, buttonYOffsetOnPressed, 0f);
        buttonShadowText.transform.localPosition = textShadowLocalPosition + new Vector3(0f, buttonYOffsetOnPressed, 0f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonText.transform.localPosition = textLocalPosition;
        buttonShadowText.transform.localPosition = textShadowLocalPosition;
    }
}
