using System;
using UnityEngine;

public class GridPuzzlePiece
{
    // 이 피스가 얼마나 회전한 각도. (아직은 고려하지 말자)
    public GridPuzzleRotateType RotateState { get; private set; }

    // 가장 왼쪽 위 좌표를 기준으로, 어떤 모양이며 어떤 좌표를 차지하는지 나타냄.
    // 예를 들어 T 모양은 (0, 0), (1, 0), (2, 0), (1, 1)
    public readonly Vector2Int[] OccupyPositions;
    public readonly int RowSize;
    public readonly int ColumnSize;

    public GridPuzzlePiece(GridPuzzleRotateType rotateState, Vector2Int[] occupyPositions)
    {
        RotateState = rotateState;
        OccupyPositions = occupyPositions;
        (RowSize, ColumnSize) = GetPieceSize(occupyPositions);
    }

    private static (int rowSize, int columnSize) GetPieceSize(Vector2Int[] occupyPositions)
    {
        var minRow = 0;
        var maxRow = 0;
        var minColumn = 0;
        var maxColumn = 0;
        foreach (var position in occupyPositions)
        {
            minRow = Math.Min(minRow, position.x);
            maxRow = Math.Max(maxRow, position.x);
            minColumn = Math.Min(minColumn, position.y);
            maxColumn = Math.Max(maxColumn, position.y);
        }

        return (maxRow - minRow + 1, maxColumn - minColumn + 1);
    }
}