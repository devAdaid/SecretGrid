using UnityEngine;

public class GridPuzzlePiece
{
    public readonly int InstanceId;

    public GridPuzzleRotateType RotateState { get; private set; }

    public readonly GridPuzzlePieceStaticData StaticData;

    public int RowSize => GetRowSize();
    public int ColumnSize => GetColumnSize();
    public readonly GridPuzzlePiecePlaceInfo AnswerPlaceInfo;

    public GridPuzzlePiece(int instanceId, GridPuzzleRotateType rotateState, GridPuzzlePieceStaticData staticData, GridPuzzlePiecePlaceInfo answerPlaceInfo)
    {
        InstanceId = instanceId;
        StaticData = staticData;
        RotateState = rotateState;
        AnswerPlaceInfo = answerPlaceInfo;
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

    private int GetRowSize()
    {
        switch (RotateState)
        {
            case GridPuzzleRotateType.Rotate0:
            case GridPuzzleRotateType.Rotate180:
                return StaticData.RowCount;
            case GridPuzzleRotateType.Rotate90:
            case GridPuzzleRotateType.Rotate270:
                return StaticData.ColumnCount;
        }

        return StaticData.RowCount;
    }

    private int GetColumnSize()
    {
        switch (RotateState)
        {
            case GridPuzzleRotateType.Rotate0:
            case GridPuzzleRotateType.Rotate180:
                return StaticData.ColumnCount;
            case GridPuzzleRotateType.Rotate90:
            case GridPuzzleRotateType.Rotate270:
                return StaticData.RowCount;
        }

        return StaticData.ColumnCount;
    }
}