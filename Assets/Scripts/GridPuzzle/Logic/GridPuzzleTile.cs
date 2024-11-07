using UnityEngine;

public struct GridPuzzleTile
{
    public readonly Vector2Int Position;
    public bool IsOccupied;
    public int OccupyingPieceId;

    public static GridPuzzleTile Invalid = new GridPuzzleTile(-1, -1, false, 0);

    public GridPuzzleTile(int row, int column, bool isOccupied, int occupyingPieceId)
    {
        Position = new Vector2Int(row, column);
        IsOccupied = isOccupied;
        OccupyingPieceId = 0;
    }
}
