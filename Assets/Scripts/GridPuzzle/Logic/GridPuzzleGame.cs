using System;
using System.Collections.Generic;
using UnityEngine;

public class GridPuzzleGame
{
    private readonly int rowCount;
    private readonly int columnCount;

    // �־��� �ǽ��� ��� ���忡 �� ä������ �� �־�� �Ѵ�.
    private readonly List<GridPuzzlePiece> pieces;

    // key�� ��ġ�� ��ġ(row, col), value�� ��ġ�� �ǽ�
    private readonly Dictionary<Vector2Int, GridPuzzlePiece> placedPieces;

    public GridPuzzleGame(int rowCount, int columnCount, List<GridPuzzlePiece> pieces)
    {
        this.rowCount = rowCount;
        this.columnCount = columnCount;
        this.pieces = pieces;
        placedPieces = new Dictionary<Vector2Int, GridPuzzlePiece>();
    }

    public GridPuzzleBoard BuildBoardSnapshot()
    {
        var occupying = new bool[rowCount, columnCount];

        foreach (var (position, piece) in placedPieces)
        {
            Vector2Int[] occupiedPositions = GetOccupiedPositions(piece, position);

            // �� ��ġ�� ���� üũ
            foreach (var occupiedPosition in occupiedPositions)
            {
                // �ش� ��ġ�� ��ȿ���� Ȯ��
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
        if (!CanPlace(piece, position))
        {
            return;
        }

        if (placedPieces.ContainsKey(position))
        {
            return;
        }

        placedPieces.Add(position, piece);
    }

    // TODO �ڵ�����, ���� ����
    public void Displace(GridPuzzlePiece piece)
    {
        Vector2Int? removeKey = null;
        foreach (var (pos, p) in placedPieces)
        {
            if (p == piece)
            {
                removeKey = pos;
            }
        }

        if (removeKey.HasValue)
        {
            placedPieces.Remove(removeKey.Value);
        }
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
