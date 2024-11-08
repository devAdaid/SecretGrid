using System.Collections.Generic;
using UnityEngine;

public class GridPuzzleGameStaticData
{
    public readonly GridPuzzleBoardStaticData BoardData;
    public readonly IReadOnlyList<GridPuzzlePieceStaticData> PieceDataList;

    public GridPuzzleGameStaticData(GridPuzzleBoardStaticData boardData, List<GridPuzzlePieceStaticData> pieceDataList)
    {
        BoardData = boardData;
        PieceDataList = pieceDataList;
    }
}

[CreateAssetMenu(fileName = "GridPuzzleGameScriptableData", menuName = "Scriptable Objects/GridPuzzleGameScriptableData")]
public class GridPuzzleGameScriptableData : ScriptableObject
{
    public GridPuzzleBoardScriptableData BoardData;
    public List<GridPuzzlePieceScriptableData> PieceDataList;

    public GridPuzzleGameStaticData ToStaticData()
    {
        var pieceDataList = new List<GridPuzzlePieceStaticData>();
        foreach (var piece in PieceDataList)
        {
            pieceDataList.Add(piece.ToStaticData());
        }

        return new GridPuzzleGameStaticData(BoardData.ToStaticData(), pieceDataList);
    }
}
