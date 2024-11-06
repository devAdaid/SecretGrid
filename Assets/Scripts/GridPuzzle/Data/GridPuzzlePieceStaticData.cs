using UnityEngine;

[CreateAssetMenu(fileName = "GridPuzzlePieceData", menuName = "Scriptable Objects/GridPuzzlePieceData")]
public class GridPuzzlePieceStaticData : ScriptableObject
{
    // TODO: https://www.sunnyvalleystudio.com/blog/unity-2d-sprite-preview-inspector-custom-editor
    [field: SerializeField]
    public Sprite Sprite;

    // ���� ���� �� ��ǥ�� ��������, � ����̸� � ��ǥ�� �����ϴ��� ��Ÿ��.
    // ���� ��� �� ����� (0, 0), (1, 0), (2, 0), (1, 1)
    [field: SerializeField]
    public Vector2Int[] OccupyPositions;

    public int RowCount
    {
        get
        {
            var minRow = 0;
            var maxRow = 0;
            foreach (var position in OccupyPositions)
            {
                minRow = Mathf.Min(minRow, position.x);
                maxRow = Mathf.Max(maxRow, position.x);
            }

            return maxRow - minRow + 1;
        }
    }

    public int ColumnCount
    {
        get
        {
            var minColumn = 0;
            var maxColumn = 0;
            foreach (var position in OccupyPositions)
            {
                minColumn = Mathf.Min(minColumn, position.y);
                maxColumn = Mathf.Max(maxColumn, position.y);
            }

            return maxColumn - minColumn + 1;
        }
    }
}
