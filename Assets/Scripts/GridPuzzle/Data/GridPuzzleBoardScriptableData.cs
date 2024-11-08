using UnityEngine;

public class GridPuzzleBoardStaticData
{
    public readonly int ColumnCount;
    public readonly int RowCount;

    public GridPuzzleBoardStaticData(int columnCount, int rowCount)
    {
        ColumnCount = columnCount;
        RowCount = rowCount;
    }
}

[CreateAssetMenu(fileName = "GridPuzzleBoardStaticData", menuName = "Scriptable Objects/GridPuzzleBoardStaticData")]
public class GridPuzzleBoardScriptableData : ScriptableObject
{
    public int ColumnCount;
    public int RowCount;

    public GridPuzzleBoardStaticData ToStaticData()
    {
        return new GridPuzzleBoardStaticData(ColumnCount, RowCount);
    }
}
