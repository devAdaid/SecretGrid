using System.Collections.Generic;
using UnityEngine;

public class GridPuzzleGameStaticData
{
    public readonly string StaticId;
    public readonly Sprite PictureSprite;
    public readonly GridPuzzleBoardStaticData BoardData;
    public readonly IReadOnlyList<GridPuzzleGamePieceStaticData> PieceDataList;

    public GridPuzzleGameStaticData(string staticId, Sprite pictureSprite, GridPuzzleBoardStaticData boardData, List<GridPuzzleGamePieceStaticData> pieceDataList)
    {
        StaticId = staticId;
        PictureSprite = pictureSprite;
        BoardData = boardData;
        PieceDataList = pieceDataList;
    }
}

[CreateAssetMenu(fileName = "GridPuzzleGameScriptableData", menuName = "Scriptable Objects/GridPuzzleGameScriptableData")]
public class GridPuzzleGameScriptableData : ScriptableObject
{
    public Sprite PictureSprite;
    public GridPuzzleBoardScriptableData BoardData;
    public List<GridPuzzleGamePieceData> PieceDataList;

    public GridPuzzleGameStaticData ToStaticData(IReadOnlyDictionary<string, GridPuzzleBoardStaticData> boardMap, IReadOnlyDictionary<string, GridPuzzlePieceStaticData> pieceMap)
    {
        var pieceDataList = new List<GridPuzzleGamePieceStaticData>();
        foreach (var gamePiece in PieceDataList)
        {
            var data = new GridPuzzleGamePieceStaticData(pieceMap[gamePiece.Piece.name], gamePiece.PlaceInfo);
            pieceDataList.Add(data);
        }

        return new GridPuzzleGameStaticData(name, PictureSprite, boardMap[BoardData.name], pieceDataList);
    }
}
