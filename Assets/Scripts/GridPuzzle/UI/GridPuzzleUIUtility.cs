using System;
using UnityEngine;

public static class GridPuzzleUIUtility
{
    public static Vector2 GetBoardCenterToTileCenterOffset(Vector2Int position, GridPuzzleBoardStaticData boardStaticData, float tileSize)
    {
        var row = position.x;
        var column = position.y;
        var startX = -((boardStaticData.ColumnCount - 1) * tileSize) / 2;
        var startY = ((boardStaticData.RowCount - 1) * tileSize) / 2;
        var posX = startX + column * tileSize;
        var posY = startY - row * tileSize;
        return new Vector2(posX, posY);
    }

    public static Vector2 GetBoardCenterToPieceCenterOffset(Vector2Int placedPosition, GridPuzzlePiece piece, GridPuzzleBoardStaticData boardStaticData, float tileSize)
    {
        var tileCenter = GetBoardCenterToTileCenterOffset(placedPosition, boardStaticData, tileSize);
        var leftOffset = tileSize / 2 * (piece.ColumnSize - 1);
        var upOffset = -tileSize / 2 * (piece.RowSize - 1);
        return tileCenter + new Vector2(leftOffset, upOffset);
    }

    public static Vector2 GetAnswerBoardCenterToPieceCenterOffset(Vector2Int placedPosition, GridPuzzlePiece piece, GridPuzzleBoardStaticData boardStaticData, float tileSize)
    {
        var tileCenter = GetBoardCenterToTileCenterOffset(placedPosition, boardStaticData, tileSize);
        var leftOffset = 0f;
        var upOffset = 0f;
        var pieceStaticRowCount = piece.StaticData.RowCount;
        var pieceStaticColCount = piece.StaticData.ColumnCount;
        switch (piece.AnswerPlaceInfo.RotateType)
        {
            case GridPuzzleRotateType.Rotate0:
                {
                    leftOffset = tileCenter.x + tileSize / 2 * (pieceStaticColCount - 1);
                    upOffset = tileCenter.y - tileSize / 2 * (pieceStaticRowCount - 1);
                    break;
                }
            case GridPuzzleRotateType.Rotate90:
                {
                    leftOffset = -tileCenter.y + tileSize / 2 * (pieceStaticRowCount - 1) + tileSize / 2 * (pieceStaticColCount - pieceStaticRowCount);
                    upOffset = tileCenter.x + tileSize / 2 * (pieceStaticColCount - 1) - tileSize / 2 * (pieceStaticColCount - pieceStaticRowCount);
                    break;
                }
            case GridPuzzleRotateType.Rotate180:
                {
                    leftOffset = -tileCenter.x - tileSize / 2 * (pieceStaticColCount - 1);
                    upOffset = -tileCenter.y + tileSize / 2 * (pieceStaticRowCount - 1);
                    break;
                }
            case GridPuzzleRotateType.Rotate270:
                {
                    leftOffset = tileCenter.y - tileSize / 2 * (pieceStaticRowCount - 1) - tileSize / 2 * (pieceStaticColCount - pieceStaticRowCount);
                    upOffset = -tileCenter.x - tileSize / 2 * (pieceStaticColCount - 1) + tileSize / 2 * (pieceStaticColCount - pieceStaticRowCount);
                    break;
                }
        }
        var result = new Vector2(leftOffset, upOffset);
        return result;
    }

    public static Vector2 GetCenterToLeftUpOffset(GridPuzzlePiece piece, float tileSize)
    {
        var leftOffset = -tileSize / 2 * (piece.ColumnSize - 1);
        var upOffset = tileSize / 2 * (piece.RowSize - 1);
        return new Vector2(leftOffset, upOffset);
    }

    public static Vector2 GetPictureLocalPosition(GridPuzzleBoardStaticData boardData, GridPuzzlePiece piece, float tileSize)
    {
        var row = piece.AnswerPlaceInfo.Position.x;
        var column = piece.AnswerPlaceInfo.Position.y;
        var rowCount = piece.StaticData.RowCount;
        var columnCount = piece.StaticData.ColumnCount;

        switch (piece.AnswerPlaceInfo.RotateType)
        {
            case GridPuzzleRotateType.Rotate0:
                return new Vector2(column * tileSize - (columnCount - 1) * tileSize / 2, -(row * tileSize) + (rowCount - 1) * tileSize / 2);
            case GridPuzzleRotateType.Rotate90:
                return new Vector2(row * tileSize - (rowCount - 1) * tileSize / 2, column * tileSize - (columnCount - 1) * tileSize / 2);
            case GridPuzzleRotateType.Rotate180:
                return new Vector2(-(column * tileSize) + (columnCount - 1) * tileSize / 2, row * tileSize - (rowCount - 1) * tileSize / 2);
            case GridPuzzleRotateType.Rotate270:
                return new Vector2(-(row * tileSize) + (rowCount - 1) * tileSize / 2, -(column * tileSize) + (columnCount - 1) * tileSize / 2);
            default:
                throw new ArgumentException("Invalid rotation angle. Must be one of 0, 90, 180, 270.");
        }
    }

    public static Vector2 GetPictureLocalPosition(
        GridPuzzleBoardStaticData boardData,
        Vector2Int inputTilePosition,
        GridPuzzlePiece piece, float tileSize)
    {
        var baseX = inputTilePosition.y * tileSize;
        var baseY = inputTilePosition.x * tileSize;
        var offsetX = 0f;
        var offsetY = 0f;

        switch (piece.AnswerPlaceInfo.RotateType)
        {
            case GridPuzzleRotateType.Rotate0:
                offsetX = 0;
                offsetY = 0;
                break;
            case GridPuzzleRotateType.Rotate90:
                offsetX = -(piece.StaticData.RowCount - 1) * tileSize;
                offsetY = 0;
                break;
            case GridPuzzleRotateType.Rotate180:
                offsetX = -(piece.StaticData.ColumnCount - 1) * tileSize;
                offsetY = -(piece.StaticData.RowCount - 1) * tileSize;
                break;
            case GridPuzzleRotateType.Rotate270:
                offsetX = 0;
                offsetY = -(piece.StaticData.ColumnCount - 1) * tileSize;
                break;
            default:
                throw new ArgumentException("Invalid rotation angle. Must be 0, 90, 180, or 270.");
        }

        var resultX = baseX + offsetX;
        var resultY = baseY + offsetY;

        return new Vector2(resultX, resultY);
    }


    public static Vector2 GetPictureLocalPositionOffset(
        GridPuzzleBoardStaticData boardData,
        Vector2Int inputTilePosition,
        GridPuzzlePiece piece, float tileSize)
    {
        var pictureCenterX = (piece.StaticData.ColumnCount - (boardData.ColumnCount - 1) / 2) * tileSize;
        var pictureCenterY = ((boardData.RowCount - 1) / 2 - piece.StaticData.RowCount) * tileSize;

        var baseX = inputTilePosition.y * tileSize;
        var baseY = inputTilePosition.x * tileSize;
        var offsetX = 0f;
        var offsetY = 0f;

        switch (piece.AnswerPlaceInfo.RotateType)
        {
            case GridPuzzleRotateType.Rotate0:
                offsetX = 0;
                offsetY = 0;
                break;
            case GridPuzzleRotateType.Rotate90:
                offsetX = -(piece.StaticData.RowCount - 1) * tileSize;
                offsetY = 0;
                break;
            case GridPuzzleRotateType.Rotate180:
                offsetX = -(piece.StaticData.ColumnCount - 1) * tileSize;
                offsetY = -(piece.StaticData.RowCount - 1) * tileSize;
                break;
            case GridPuzzleRotateType.Rotate270:
                offsetX = 0;
                offsetY = -(piece.StaticData.ColumnCount - 1) * tileSize;
                break;
            default:
                throw new ArgumentException("Invalid rotation angle. Must be 0, 90, 180, or 270.");
        }

        var resultX = baseX + offsetX;
        var resultY = baseY + offsetY;

        return new Vector2(resultX, resultY);
    }
}