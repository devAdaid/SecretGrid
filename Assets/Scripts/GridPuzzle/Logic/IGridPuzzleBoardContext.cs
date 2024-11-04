using UnityEngine;

public interface IGridPuzzleBoardContext
{
    int RowCount { get; }
    int ColumnCount { get; }
    bool IsValidPosition(Vector2Int position);
}