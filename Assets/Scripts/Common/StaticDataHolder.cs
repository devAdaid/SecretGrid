using System.Collections.Generic;
using UnityEngine;

public class StaticDataHolder : PersistentSingleton<StaticDataHolder>
{
    private IReadOnlyDictionary<string, GridPuzzlePieceStaticData> pieceById;
    private IReadOnlyDictionary<string, GridPuzzleBoardStaticData> boardById;
    private IReadOnlyDictionary<string, GridPuzzleGameStaticData> gameById;

    protected override void Initialize()
    {
        var pieceMap = new Dictionary<string, GridPuzzlePieceStaticData>();
        var pieceDataList = Resources.LoadAll<GridPuzzlePieceScriptableData>("Data/Piece");
        foreach (var pieceData in pieceDataList)
        {
            pieceMap.Add(pieceData.name, pieceData.ToStaticData());
        }
        pieceById = pieceMap;

        var boardMap = new Dictionary<string, GridPuzzleBoardStaticData>();
        var boardDataList = Resources.LoadAll<GridPuzzleBoardScriptableData>("Data/Board");
        foreach (var boardData in boardDataList)
        {
            boardMap.Add(boardData.name, boardData.ToStaticData());
        }
        boardById = boardMap;

        var gameMap = new Dictionary<string, GridPuzzleGameStaticData>();
        var gameDataList = Resources.LoadAll<GridPuzzleGameScriptableData>("Data/Game");
        foreach (var gameData in gameDataList)
        {
            gameMap.Add(gameData.name, gameData.ToStaticData(boardMap, pieceMap));
        }
        gameById = gameMap;
    }

    public GridPuzzleGameStaticData GetGameData(string staticId)
    {
        return gameById[staticId];
    }
}
