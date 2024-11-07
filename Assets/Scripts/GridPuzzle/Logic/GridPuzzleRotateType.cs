using UnityEngine;

public enum GridPuzzleRotateType
{
    Rotate0,
    Rotate90,
    Rotate180,
    Rotate270,
}


public static class GridPuzzleRotateTypeExtension
{
    public static GridPuzzleRotateType Rotate90(this GridPuzzleRotateType rotateType)
    {
        switch (rotateType)
        {
            case GridPuzzleRotateType.Rotate0:
                return GridPuzzleRotateType.Rotate90;
            case GridPuzzleRotateType.Rotate90:
                return GridPuzzleRotateType.Rotate180;
            case GridPuzzleRotateType.Rotate180:
                return GridPuzzleRotateType.Rotate270;
            case GridPuzzleRotateType.Rotate270:
                return GridPuzzleRotateType.Rotate0;
        }
        return GridPuzzleRotateType.Rotate0;
    }

    public static Vector3 ToEulerAngles(this GridPuzzleRotateType rotateType)
    {
        switch (rotateType)
        {
            case GridPuzzleRotateType.Rotate0:
                return Vector3.zero;
            case GridPuzzleRotateType.Rotate90:
                return new Vector3(0, 0, -90);
            case GridPuzzleRotateType.Rotate180:
                return new Vector3(0, 0, 180);
            case GridPuzzleRotateType.Rotate270:
                return new Vector3(0, 0, 90);
        }
        return Vector3.zero;
    }
}
