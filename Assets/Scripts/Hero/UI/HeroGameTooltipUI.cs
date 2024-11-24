using TMPro;
using UnityEngine;

public class HeroGameTooltipUI : MonoSingleton<HeroGameTooltipUI>
{
    [SerializeField]
    private RectTransform tooltipRectTransform;

    [SerializeField]
    private TMP_Text tooltipText;

    private Canvas canvas;
    private RectTransform rectTransform;

    private Vector2 offset;

    private void Awake()
    {
        canvas = FindAnyObjectByType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        tooltipRectTransform.gameObject.SetActive(false);
    }

    public void Show(Vector2 tooltipOffset, string text)
    {
        tooltipRectTransform.gameObject.SetActive(true);
        offset = tooltipOffset;
        tooltipText.text = text;
        UpdatePosition();
    }

    public void Hide()
    {
        tooltipRectTransform.gameObject.SetActive(false);
    }

    // Update is called once per frame
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
