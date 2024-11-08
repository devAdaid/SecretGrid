using UnityEngine;
using UnityEngine.UI;

public class GridPuzzlePieceControlInitializeParameter : ISpawnableObjectInitializeParameter
{
    public readonly GridPuzzlePiece Piece;
    public readonly float TileSize;

    public GridPuzzlePieceControlInitializeParameter(GridPuzzlePiece piece, float tileSize)
    {
        Piece = piece;
        TileSize = tileSize;
    }
}

public class GridPuzzlePieceControl : MonoBehaviour, ISpawnableObject
{
    public GridPuzzlePiece Piece { get; private set; }

    [SerializeField]
    private Image pieceImage;

    [SerializeField]
    private RectTransform rectTransform;

    public void Initialize(ISpawnableObjectInitializeParameter parameter)
    {
        if (parameter is not GridPuzzlePieceControlInitializeParameter param)
        {
            return;
        }

        Piece = param.Piece;
        pieceImage.sprite = param.Piece.StaticData.Sprite;
        transform.localEulerAngles = Piece.RotateState.ToEulerAngles();
        rectTransform.sizeDelta = new Vector2(param.TileSize * param.Piece.ColumnSize, param.TileSize * param.Piece.RowSize);
    }

    public void Despawn()
    {
        ObjectPoolHolder.I.PiecePool.Despawn(this);
    }

    public void SetActive(bool active)
    {
        transform.localEulerAngles = Piece.RotateState.ToEulerAngles();
        pieceImage.gameObject.SetActive(active);
    }

    public void OnClick()
    {
        GridPuzzleUI.I.SetHoldingPiece(Piece);
    }
}
