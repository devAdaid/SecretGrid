using System.Collections.Generic;
using UnityEngine;
public class GridPuzzlePieceStaticData
{
    public readonly Sprite Sprite;
    public readonly Vector2Int[] OccupyPositions;
    public readonly int RowCount;
    public readonly int ColumnCount;

    public GridPuzzlePieceStaticData(Sprite sprite, Vector2Int[] occupyPositions, int rowCount, int columnCount)
    {
        this.Sprite = sprite;
        this.OccupyPositions = occupyPositions;
        this.RowCount = rowCount;
        this.ColumnCount = columnCount;
    }
}

[CreateAssetMenu(fileName = "GridPuzzlePieceData", menuName = "Scriptable Objects/GridPuzzlePieceData")]
public class GridPuzzlePieceScriptableData : ScriptableObject
{
    public int ColumnCount;
    public int RowCount;
    public bool[] OccupyList;
    public Sprite Sprite;

    public GridPuzzlePieceStaticData ToStaticData()
    {
        var occupyPositionList = new List<Vector2Int>();
        for (var row = 0; row < RowCount; row++)
        {
            for (var col = 0; col < ColumnCount; col++)
            {
                var index = row * ColumnCount + col;
                if (OccupyList[index])
                {
                    occupyPositionList.Add(new Vector2Int(row, col));
                }
            }
        }

        return new GridPuzzlePieceStaticData(Sprite, occupyPositionList.ToArray(), RowCount, ColumnCount);
    }
}
