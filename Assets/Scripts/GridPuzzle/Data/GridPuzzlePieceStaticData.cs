using UnityEngine;

[CreateAssetMenu(fileName = "GridPuzzlePieceData", menuName = "Scriptable Objects/GridPuzzlePieceData")]
public class GridPuzzlePieceStaticData : ScriptableObject
{
    // TODO: https://www.sunnyvalleystudio.com/blog/unity-2d-sprite-preview-inspector-custom-editor
    [field: SerializeField]
    public Sprite Sprite;

    // 가장 왼쪽 위 좌표를 기준으로, 어떤 모양이며 어떤 좌표를 차지하는지 나타냄.
    // 예를 들어 ㅜ 모양은 (0, 0), (1, 0), (2, 0), (1, 1)
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
