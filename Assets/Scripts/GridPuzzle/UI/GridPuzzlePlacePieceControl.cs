using UnityEngine;
using UnityEngine.UI;

public class GridPuzzlePlacePieceControl : MonoBehaviour
{
    [SerializeField]
    private Image pieceImage;

    public GridPuzzlePiece Piece { get; private set; }
    public RectTransform RectTransform { get; private set; }

    private float tileSize;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(GridPuzzlePiece piece, float tileSize)
    {
        Piece = piece;
        this.tileSize = tileSize;
        var (rowSize, colSize) = piece.GetPieceSize();
        RectTransform.sizeDelta = new Vector2(colSize * tileSize, rowSize * tileSize);
        pieceImage.sprite = piece.StaticData.Sprite;
    }

    public void SetActive(bool active)
    {
        transform.localEulerAngles = Piece.RotateState.ToEulerAngles();
        gameObject.SetActive(active);
    }

    public Vector2 GetLeftUpToCenterOffset()
    {
        var (rowSize, columnSize) = Piece.GetPieceSize();
        var leftOffset = tileSize / 2 * (columnSize - 1);
        var upOffset = -tileSize / 2 * (rowSize - 1);
        return new Vector2(leftOffset, upOffset);
    }
}
