using UnityEngine;

public class GridPuzzlePlacePieceControl : GridPuzzlePlacePieceControlBase, ISpawnableObject
{
    public void Initialize(ISpawnableObjectInitializeParameter parameter)
    {
        if (parameter is not GridPuzzlePieceControlInitializeParameter param)
        {
            return;
        }

        DoInitialize(param);
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
