using UnityEngine;

public static class GridPuzzleUIUtility
{
    public static Vector2 GetCenterToLeftUpOffset(GridPuzzlePiece piece, float tileSize)
    {
        var leftOffset = -tileSize / 2 * (piece.ColumnSize - 1);
        var upOffset = tileSize / 2 * (piece.RowSize - 1);
        return new Vector2(leftOffset, upOffset);
    }
}