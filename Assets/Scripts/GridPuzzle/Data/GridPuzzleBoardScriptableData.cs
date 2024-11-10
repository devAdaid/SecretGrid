using UnityEngine;

public class GridPuzzleBoardStaticData
{
    public readonly string StaticId;
    public readonly int ColumnCount;
    public readonly int RowCount;

    public GridPuzzleBoardStaticData(string staticId, int columnCount, int rowCount)
    {
        StaticId = staticId;
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
        return new GridPuzzleBoardStaticData(name, ColumnCount, RowCount);
    }
}
