using UnityEngine;

[CreateAssetMenu(fileName = "GridPuzzleBoardStaticData", menuName = "Scriptable Objects/GridPuzzleBoardStaticData")]
public class GridPuzzleBoardStaticData : ScriptableObject
{
    [field: SerializeField]
    public int ColumnCount;

    [field: SerializeField]
    public int RowCount;
}
