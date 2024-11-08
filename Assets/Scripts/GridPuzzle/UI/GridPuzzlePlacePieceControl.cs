using UnityEngine;
using UnityEngine.UI;

public class GridPuzzlePlacePieceControlInitializeParameter : ISpawnableObjectInitializeParameter
{
    public readonly GridPuzzlePiece Piece;
    public readonly float TileSize;

    public GridPuzzlePlacePieceControlInitializeParameter(GridPuzzlePiece piece, float tileSize)
    {
        Piece = piece;
        TileSize = tileSize;
    }
}

public class GridPuzzlePlacePieceControl : MonoBehaviour, ISpawnableObject
{
    [SerializeField]
    private Image pieceImage;

    [SerializeField]
    public RectTransform rectTransform;

    public GridPuzzlePiece Piece { get; private set; }

    private float tileSize;

    public void Initialize(ISpawnableObjectInitializeParameter parameter)
    {
        if (parameter is not GridPuzzlePlacePieceControlInitializeParameter param)
        {
            return;
        }

        Piece = param.Piece;
        tileSize = param.TileSize;

        rectTransform.sizeDelta = new Vector2(Piece.ColumnSize * tileSize, Piece.RowSize * tileSize);
        pieceImage.sprite = param.Piece.StaticData.Sprite;
    }

    public void Despawn()
    {
        ObjectPoolHolder.I.PlacePiecePool.Despawn(this);
    }

    public void SetActive(bool active)
    {
        transform.localEulerAngles = Piece.RotateState.ToEulerAngles();
        gameObject.SetActive(active);
    }

    public Vector2 GetLeftUpToCenterOffset()
    {
        var leftOffset = tileSize / 2 * (Piece.ColumnSize - 1);
        var upOffset = -tileSize / 2 * (Piece.RowSize - 1);
        return new Vector2(leftOffset, upOffset);
    }
}
