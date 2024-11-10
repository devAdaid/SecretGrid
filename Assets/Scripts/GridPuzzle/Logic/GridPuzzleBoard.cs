using System.Collections.Generic;
using UnityEngine;

public struct GridPuzzleBoard : IGridPuzzleBoardContext
{
    public int RowCount => TileArray.GetLength(0);
    public int ColumnCount => TileArray.GetLength(1);

    public readonly GridPuzzleTile[,] TileArray;

    public GridPuzzleBoard(GridPuzzleBoardStaticData staticData, List<(GridPuzzlePiece piece, Vector2Int position)> placedPiecesWithPositions)
    {
        var rowCount = staticData.RowCount;
        var columnCount = staticData.ColumnCount;
        TileArray = new GridPuzzleTile[rowCount, columnCount];

        for (var row = 0; row < rowCount; row++)
        {
            for (var column = 0; column < columnCount; column++)
            {
                TileArray[row, column] = new GridPuzzleTile(row, column, false, 0);
            }
        }

        foreach (var (piece, position) in placedPiecesWithPositions)
        {
            var occupiedPositions = piece.GetOccupyPositions(position);

            // 각 위치에 대해 체크
            foreach (var occupiedPosition in occupiedPositions)
            {
                // 해당 위치가 유효한지 확인
                if (!IsValidPosition(occupiedPosition))
                {
                    continue;
                }

                var tile = TileArray[occupiedPosition.x, occupiedPosition.y];
                tile.IsOccupied = true;
                tile.OccupyingPieceId = piece.InstanceId;
                TileArray[occupiedPosition.x, occupiedPosition.y] = tile;
            }
        }
    }

    public bool CanPlace(GridPuzzlePiece piece, Vector2Int position)
    {
        Vector2Int[] occupiedPositions = piece.GetOccupyPositions(position);

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

    public bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < RowCount && position.y < ColumnCount;
    }

    public bool TryGetTile(Vector2Int position, out GridPuzzleTile tile)
    {
        if (!IsValidPosition(position))
        {
            tile = GridPuzzleTile.Invalid;
            return false;
        }

        tile = TileArray[position.x, position.y];
        return true;
    }

    public bool IsAllOccupied()
    {
        foreach (var tile in TileArray)
        {
            if (!tile.IsOccupied)
            {
                return false;
            }
        }

        return true;
    }
}
