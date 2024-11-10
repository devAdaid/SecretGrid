using System;
using UnityEngine;

[Serializable]
public struct GridPuzzlePiecePlaceInfo
{
    public GridPuzzleRotateType RotateType;
    public Vector2Int Position;

    public GridPuzzlePiecePlaceInfo(GridPuzzleRotateType rotateType, Vector2Int position)
    {
        RotateType = rotateType;
        Position = position;
    }
}