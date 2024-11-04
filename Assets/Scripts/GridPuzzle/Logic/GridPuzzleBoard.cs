using System;
using UnityEngine;

public struct GridPuzzleBoard : IGridPuzzleBoardContext
{
    public int RowCount => TileArray.GetLength(0);
    public int ColumnCount => TileArray.GetLength(1);

    public readonly GridPuzzleTile[,] TileArray;

    public GridPuzzleBoard(bool[,] tileOccupying)
    {
        var rowCount = tileOccupying.GetLength(0);
        var columnCount = tileOccupying.GetLength(1);

        TileArray = new GridPuzzleTile[rowCount, columnCount];
        for (var row = 0; row < rowCount; row++)
        {
            for (var column = 0; column < columnCount; column++)
            {
                TileArray[row, column] = new GridPuzzleTile(row, column, tileOccupying[row, column]);
            }
        }
    }

    public GridPuzzleBoard(int rowCount, int columnCount)
    {
        TileArray = new GridPuzzleTile[rowCount, columnCount];
        for (var row = 0; row < rowCount; row++)
        {
            for (var column = 0; column < columnCount; column++)
            {
                TileArray[row, column] = new GridPuzzleTile(row, column, false);
            }
        }
    }

    public bool CanPlace(GridPuzzlePiece piece, Vector2Int position)
    {
        Vector2Int[] occupiedPositions = GetOccupiedPositions(piece, position);

        foreach (var occupiedPosition in occupiedPositions)
        {
            if (!IsValidPosition(occupiedPosition))
            {
                return false;
            }

            // 해당 위치가 이미 차지되고 있는지 확인
            if (TileArray[occupiedPosition.x, occupiedPosition.y].IsOccupied)
            {
                return false;
            }
        }

        return true;
    }

    private Vector2Int[] GetOccupiedPositions(GridPuzzlePiece piece, Vector2Int basePosition)
    {
        Vector2Int[] rotatedPositions = new Vector2Int[piece.OccupyPositions.Length];

        for (int i = 0; i < piece.OccupyPositions.Length; i++)
        {
            Vector2Int originalPos = piece.OccupyPositions[i];
            Vector2Int rotatedPos = RotatePosition(originalPos, piece.RotateState);

            rotatedPositions[i] = new Vector2Int(rotatedPos.x + basePosition.x, rotatedPos.y + basePosition.y);
        }

        return rotatedPositions;
    }

    // 주어진 원래 위치를 회전 상태에 따라 회전시키는 메서드
    private Vector2Int RotatePosition(Vector2Int position, GridPuzzleRotateType rotateState)
    {
        return rotateState switch
        {
            GridPuzzleRotateType.Rotate0 => position,
            GridPuzzleRotateType.Rotate90 => new Vector2Int(-position.y, position.x),
            GridPuzzleRotateType.Rotate180 => new Vector2Int(-position.x, -position.y),
            GridPuzzleRotateType.Rotate270 => new Vector2Int(position.y, -position.x),
            _ => throw new ArgumentOutOfRangeException(nameof(rotateState), rotateState, null)
        };
    }


    public bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < RowCount && position.y < ColumnCount;
    }
}
