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
        switch (RotateState)
        {
            case GridPuzzleRotateType.Rotate0:
            case GridPuzzleRotateType.Rotate180:
                return (StaticData.RowCount, StaticData.ColumnCount);
            case GridPuzzleRotateType.Rotate90:
            case GridPuzzleRotateType.Rotate270:
                return (StaticData.ColumnCount, StaticData.RowCount);
        }

        return (StaticData.RowCount, StaticData.ColumnCount);
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

    public void Rotate90()
    {
        RotateState = RotateState.Rotate90();
    }
}