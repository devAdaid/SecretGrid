using UnityEngine;
using UnityEngine.UI;

public class GridPuzzlePieceControl : MonoBehaviour
{
    public GridPuzzlePiece Piece { get; private set; }

    [SerializeField]
    private Image pieceImage;

    private IGridPuzzleUI puzzleUI;

    public void Initialize(GridPuzzlePiece piece, IGridPuzzleUI puzzleUI)
    {
        Piece = piece;
        this.puzzleUI = puzzleUI;

        pieceImage.sprite = piece.StaticData.Sprite;
        pieceImage.rectTransform.localEulerAngles = piece.RotateState.ToEulerAngles();
        GetComponent<RectTransform>().sizeDelta = new Vector2(puzzleUI.TileSize * piece.StaticData.ColumnCount, puzzleUI.TileSize * piece.StaticData.RowCount);
    }

    public void SetActive(bool active)
    {
        pieceImage.rectTransform.localEulerAngles = Piece.RotateState.ToEulerAngles();
        pieceImage.gameObject.SetActive(active);
    }

    public void OnClick()
    {
        puzzleUI.SetHoldingPiece(Piece);
    }
}
