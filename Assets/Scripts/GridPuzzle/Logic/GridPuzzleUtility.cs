using System.Linq;
using UnityEngine;

public static class GridPuzzleUtility
{
    public static Vector2Int[] GetRotatedPositions(Vector2Int[] occupyPositions, GridPuzzleRotateType rotateState)
    {
        // 각 좌표를 회전
        Vector2Int[] rotatedPositions = occupyPositions
            .Select(position => RotatePosition(position, rotateState))
            .ToArray();

        // 회전한 좌표들의 최소값을 구해, 음수가 없도록 이동
        int minX = rotatedPositions.Min(pos => pos.x);
        int minY = rotatedPositions.Min(pos => pos.y);

        // 최솟값이 0 이상이 되도록 조정
        Vector2Int offset = new Vector2Int(minX < 0 ? -minX : 0, minY < 0 ? -minY : 0);

        for (int i = 0; i < rotatedPositions.Length; i++)
        {
            rotatedPositions[i] += offset;
        }

        return rotatedPositions;
    }

    private static Vector2Int RotatePosition(Vector2Int position, GridPuzzleRotateType rotateState)
    {
        int x = position.x;
        int y = position.y;

        switch (rotateState)
        {
            case GridPuzzleRotateType.Rotate90:
                return new Vector2Int(y, -x);
            case GridPuzzleRotateType.Rotate180:
                return new Vector2Int(-x, -y);
            case GridPuzzleRotateType.Rotate270:
                return new Vector2Int(-y, x);
            case GridPuzzleRotateType.Rotate0:
            default:
                return new Vector2Int(x, y);
        }
    }
}