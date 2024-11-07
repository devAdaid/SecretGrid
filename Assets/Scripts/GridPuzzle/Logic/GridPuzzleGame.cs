using System;
using System.Collections.Generic;
using UnityEngine;

public class GridPuzzleGame
{
    private readonly int rowCount;
    private readonly int columnCount;

    // �־��� �ǽ��� ��� ���忡 �� ä������ �� �־�� �Ѵ�.
    public readonly List<GridPuzzlePiece> Pieces;

    // key�� ��ġ�� ��ġ(row, col), value�� ��ġ�� �ǽ�
    public readonly Dictionary<int, GridPuzzlePiece> PieceMap;
    public readonly Dictionary<int, Vector2Int> PlacedPieces;

    public GridPuzzleGame(int rowCount, int columnCount, List<GridPuzzlePieceStaticData> pieceDataList)
    {
        this.rowCount = rowCount;
        this.columnCount = columnCount;

        Pieces = new List<GridPuzzlePiece>();
        PieceMap = new Dictionary<int, GridPuzzlePiece>();
        for (int i = 0; i < pieceDataList.Count; i++)
        {
            var piece = new GridPuzzlePiece(i + 1, GridPuzzleRotateType.Rotate0, pieceDataList[i]);
            Pieces.Add(piece);
            PieceMap.Add(piece.InstanceId, piece);
        }

        PlacedPieces = new Dictionary<int, Vector2Int>();
    }

    public GridPuzzleBoard BuildBoardSnapshot()
    {
        var placedPieceWithPositions = new List<(GridPuzzlePiece piece, Vector2Int position)>();
        foreach (var (pieceId, position) in PlacedPieces)
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
        // ���� �� ���� �ڸ�
        if (!CanPlace(piece, position))
        {
            return;
        }

        // �̹� ��ġ�� �ǽ�
        if (PlacedPieces.ContainsKey(piece.InstanceId))
        {
            return;
        }

        PlacedPieces[piece.InstanceId] = position;
    }

    public void Displace(int pieceId)
    {
        PlacedPieces.Remove(pieceId);
    }

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
        return position.x >= 0 && position.y >= 0 && position.x < rowCount && position.y < columnCount;
    }
}
