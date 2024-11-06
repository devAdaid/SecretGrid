using System;
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
        var occupying = new bool[rowCount, columnCount];

        foreach (var (pieceId, position) in PlacedPieces)
        {
            var piece = PieceMap[pieceId];
            var occupiedPositions = piece.GetOccupyPositions(position);

            // 각 위치에 대해 체크
            foreach (var occupiedPosition in occupiedPositions)
            {
                // 해당 위치가 유효한지 확인
                if (!IsValidPosition(occupiedPosition))
                {
                    continue;
                }

                occupying[occupiedPosition.x, occupiedPosition.y] = true;
            }
        }

        return new GridPuzzleBoard(occupying);
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
        if (PlacedPieces.ContainsKey(piece.InstanceId))
        {
            return;
        }

        PlacedPieces[piece.InstanceId] = position;
    }

    // TODO 코드정리, 구조 변경
    public void Displace(GridPuzzlePiece piece)
    {
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
