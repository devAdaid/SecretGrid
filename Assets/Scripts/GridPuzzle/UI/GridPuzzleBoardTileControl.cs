using UnityEngine;
using UnityEngine.UI;

public class GridPuzzleBoardTileControl : MonoBehaviour
{
    [SerializeField]
    private Image tileImage;
    public RectTransform RectTransform { get; private set; }

    private Vector2Int position;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(Vector2Int pos, float tileSize)
    {
        position = pos;
        RectTransform.sizeDelta = new Vector2(tileSize, tileSize);
    }

    public void SetSize(float tileSize)
    {
        RectTransform.sizeDelta = new Vector2(tileSize, tileSize);
    }

    public void SetOccupy(bool occupy)
    {
        tileImage.color = occupy ? Color.red : Color.white;
    }

    public void SetPreview(bool preview)
    {
        var color = preview ? Color.yellow : tileImage.color;
        color.a = preview ? 0.25f : 1f;
        tileImage.color = color;
    }
}
