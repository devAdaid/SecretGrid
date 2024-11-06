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

    public bool IsOccupiedByPiece(Vector2Int position, out GridPuzzlePiece piece)
    {
        if (!IsValidPosition(position))
        {
            piece = null;
            return false;
        }

        var tile = TileArray[position.x, position.y];
        if (!tile.IsOccupied)
        {
            piece = null;
            return false;
        }

        piece = tile.OccupyingPiece;
        return true;
    }
}
