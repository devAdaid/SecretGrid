using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroGameTooltipUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform tooltipRectTransform;

    [SerializeField]
    private TMP_Text tooltipText;

    [SerializeField]
    private LayoutElement layoutElement;

    [SerializeField]
    private Canvas canvas;

    private Vector2 offset;

    private void Awake()
    {
        tooltipRectTransform.gameObject.SetActive(false);
    }

    public void Show(string text, Vector2 offset, float preferedWidth)
    {
        tooltipRectTransform.gameObject.SetActive(true);
        this.offset = offset;
        layoutElement.preferredWidth = preferedWidth;
        tooltipText.text = text;
        UpdatePosition();
    }

    public void Hide()
    {
        tooltipRectTransform.gameObject.SetActive(false);
    }

    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        var mousePosition = Input.mousePosition;

        // 마우스 위치를 Canvas의 로컬 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            mousePosition,
            Camera.main,
            out var localPoint
        );

        var tooltipPosition = localPoint + offset;

        var canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
        var tooltipSize = tooltipRectTransform.sizeDelta;

        tooltipPosition.x = Mathf.Clamp(tooltipPosition.x, -canvasSize.x / 2 + tooltipSize.x / 2, canvasSize.x / 2 - tooltipSize.x / 2);
        tooltipPosition.y = Mathf.Clamp(tooltipPosition.y, -canvasSize.y / 2 + tooltipSize.y / 2, canvasSize.y / 2 - tooltipSize.y / 2);

        tooltipRectTransform.localPosition = tooltipPosition;
    }
}
