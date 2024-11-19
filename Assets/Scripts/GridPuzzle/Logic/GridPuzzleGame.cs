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
    public readonly Dictionary<int, Vector2Int> PlacedPiecePositionMap;
    private readonly Dictionary<int, GridPuzzlePiecePlaceInfo> piecePlaceAnswerMap;

    public readonly GridPuzzleGameStaticData StaticData;

    public GridPuzzleGame(GridPuzzleGameStaticData data)
    {
        StaticData = data;
        rowCount = data.BoardData.RowCount;
        columnCount = data.BoardData.ColumnCount;

        Pieces = new List<GridPuzzlePiece>();
        PieceMap = new Dictionary<int, GridPuzzlePiece>();
        piecePlaceAnswerMap = new Dictionary<int, GridPuzzlePiecePlaceInfo>();
        for (int i = 0; i < data.PieceDataList.Count; i++)
        {
            var instanceId = i + 1;
            var piece = new GridPuzzlePiece(instanceId, GridPuzzleRotateType.Rotate0, data.PieceDataList[i].Piece, data.PieceDataList[i].PlaceInfo);
            Pieces.Add(piece);
            PieceMap.Add(instanceId, piece);
            piecePlaceAnswerMap.Add(instanceId, data.PieceDataList[i].PlaceInfo);
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

        return new GridPuzzleBoard(StaticData.BoardData, placedPieceWithPositions);
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
        foreach (var (pieceInstanceId, answer) in piecePlaceAnswerMap)
        {
            var pieceAnwerRotateType = answer.RotateType;
            if (!PlacedPiecePositionMap.TryGetValue(pieceInstanceId, out var placedPosition) || answer.Position != placedPosition)
            {
                return false;
            }

            if (!PieceMap.TryGetValue(pieceInstanceId, out var piece) || answer.RotateType != piece.RotateState)
            {
                return false;
            }
        }

        return true;
    }
}
