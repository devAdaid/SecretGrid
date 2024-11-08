using System.Collections.Generic;
using UnityEngine;

public class GridPuzzleGame
{
    private readonly int rowCount;
    private readonly int columnCount;

    // 주어진 피스를 모두 보드에 꽉 채워넣을 수 있어야 한다.
    public readonly List<GridPuzzlePiece> Pieces;

    // key는 배치된 위치(row, col), value는 배치된 피스
    public readonly Dictionary<int, GridPuzzlePiece> PieceMap;
    public readonly Dictionary<int, Vector2Int> PlacedPiecePositionMap;

    public GridPuzzleGame(GridPuzzleGameStaticData data)
    {
        this.rowCount = data.BoardData.RowCount;
        this.columnCount = data.BoardData.ColumnCount;

        Pieces = new List<GridPuzzlePiece>();
        PieceMap = new Dictionary<int, GridPuzzlePiece>();
        for (int i = 0; i < data.PieceDataList.Count; i++)
        {
            var piece = new GridPuzzlePiece(i + 1, GridPuzzleRotateType.Rotate0, data.PieceDataList[i]);
            Pieces.Add(piece);
            PieceMap.Add(piece.InstanceId, piece);
        }

        PlacedPiecePositionMap = new Dictionary<int, Vector2Int>();
    }

    public GridPuzzleBoard BuildBoardSnapshot()
    {
        var placedPieceWithPositions = new List<(GridPuzzlePiece piece, Vector2Int position)>();
        foreach (var (pieceId, position) in PlacedPiecePositionMap)
        {
            placedPieceWithPositions.Add((PieceMap[pieceId], position));
        }

        return new GridPuzzleBoard(rowCount, columnCount, placedPieceWithPositions);
    }

    public bool CanPlace(GridPuzzlePiece piece, Vector2Int position)
    {
        var board = BuildBoardSnapshot();
        return board.CanPlace(piece, position);
    }

    public void Place(GridPuzzlePiece piece, Vector2Int position)
    {
        // 놓을 수 없는 자리
        if (!CanPlace(piece, position))
        {
            return;
        }

        // 이미 배치된 피스
        if (PlacedPiecePositionMap.ContainsKey(piece.InstanceId))
        {
            return;
        }

        PlacedPiecePositionMap[piece.InstanceId] = position;
    }

    public void Displace(int pieceId)
    {
        PlacedPiecePositionMap.Remove(pieceId);
    }

    public bool IsCleared()
    {
        var boardSnapshot = BuildBoardSnapshot();
        return boardSnapshot.IsAllOccupied();
    }
}
