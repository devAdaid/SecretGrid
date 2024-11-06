using UnityEngine;

public struct GridPuzzleTile
{
    public readonly Vector2Int Position;
    public bool IsOccupied { get; private set; }
    public GridPuzzlePiece OccupyingPiece { get; private set; }

    public GridPuzzleTile(int row, int column, bool isOccupied)
    {
        Position = new Vector2Int(row, column);
        IsOccupied = isOccupied;
        OccupyingPiece = null;
    }

    public void SetOccupied(bool isOccupied, GridPuzzlePiece occupyingPiece)
    {
        IsOccupied = isOccupied;
        OccupyingPiece = occupyingPiece;
    }
}
