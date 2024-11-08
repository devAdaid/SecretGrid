using UnityEngine;

public static class GridPuzzleUIUtility
{
    public static Vector2 GetCenterToLeftUpOffset(GridPuzzlePiece piece, float tileSize)
    {
        var (rowSize, columnSize) = piece.GetPieceSize();
        var leftOffset = -tileSize / 2 * (columnSize - 1);
        var upOffset = tileSize / 2 * (rowSize - 1);
        return new Vector2(leftOffset, upOffset);
    }
}