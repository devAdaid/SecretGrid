using UnityEngine;

public class GridPuzzlePiece
{
    public readonly int InstanceId;

    public GridPuzzleRotateType RotateState { get; private set; }

    public readonly GridPuzzlePieceStaticData StaticData;

    public GridPuzzlePiece(int instanceId, GridPuzzleRotateType rotateState, GridPuzzlePieceStaticData staticData)
    {
        InstanceId = instanceId;
        StaticData = staticData;
        RotateState = rotateState;
    }

    public (int rowSize, int columnSize) GetPieceSize()
    {
        // TODO: 회전 적용
        var minRow = 0;
        var maxRow = 0;
        var minColumn = 0;
        var maxColumn = 0;
        foreach (var position in StaticData.OccupyPositions)
        {
            minRow = Mathf.Min(minRow, position.x);
            maxRow = Mathf.Max(maxRow, position.x);
            minColumn = Mathf.Min(minColumn, position.y);
            maxColumn = Mathf.Max(maxColumn, position.y);
        }

        return (maxRow - minRow + 1, maxColumn - minColumn + 1);
    }

    public Vector2Int[] GetOccupyPositions(Vector2Int basePosition)
    {
        var occupyPositions = GridPuzzleUtility.GetRotatedPositions(StaticData.OccupyPositions, RotateState);
        for (int i = 0; i < occupyPositions.Length; i++)
        {
            var occupyPosition = occupyPositions[i];
            occupyPositions[i] = new Vector2Int(occupyPosition.x + basePosition.x, occupyPosition.y + basePosition.y);
        }

        return occupyPositions;
    }
}