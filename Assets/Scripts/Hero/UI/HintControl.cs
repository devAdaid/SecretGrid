using UnityEngine;

public class HintControl : MonoBehaviour
{
    [SerializeField]
    private Vector2 offset;

    [SerializeField]
    private RectTransform tooltipRectTransform;

    private Canvas canvas;
    private RectTransform rectTransform;

    private void Awake()
    {
        canvas = FindAnyObjectByType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        // 마우스 위치를 Canvas의 로컬 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            mousePosition,
            Camera.main,
            out Vector2 localPoint
        );

        Vector2 tooltipPosition = localPoint + offset;

        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
        Vector2 tooltipSize = tooltipRectTransform.sizeDelta;

        tooltipPosition.x = Mathf.Clamp(tooltipPosition.x, -canvasSize.x / 2 + tooltipSize.x / 2, canvasSize.x / 2 - tooltipSize.x / 2);
        tooltipPosition.y = Mathf.Clamp(tooltipPosition.y, -canvasSize.y / 2 + tooltipSize.y / 2, canvasSize.y / 2 - tooltipSize.y / 2);

        rectTransform.localPosition = tooltipPosition;
    }
}
