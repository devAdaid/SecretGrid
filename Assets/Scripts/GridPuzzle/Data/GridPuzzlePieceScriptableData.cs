using System.Collections.Generic;
using UnityEngine;

public class GridPuzzlePieceStaticData
{
    public readonly string StaticId;
    public readonly Sprite Sprite;
    public readonly Vector2Int[] OccupyPositions;
    public readonly int RowCount;
    public readonly int ColumnCount;

    public GridPuzzlePieceStaticData(string staticId, Sprite sprite, Vector2Int[] occupyPositions, int rowCount, int columnCount)
    {
        StaticId = staticId;
        Sprite = sprite;
        OccupyPositions = occupyPositions;
        RowCount = rowCount;
        ColumnCount = columnCount;
    }
}

[CreateAssetMenu(fileName = "GridPuzzlePieceData", menuName = "Scriptable Objects/GridPuzzlePieceData")]
public class GridPuzzlePieceScriptableData : ScriptableObject
{
    public int ColumnCount;
    public int RowCount;
    public bool[] OccupyList;
    public Sprite Sprite;

    public GridPuzzlePieceStaticData ToStaticData()
    {
        var occupyPositionList = new List<Vector2Int>();
        for (var row = 0; row < RowCount; row++)
        {
            for (var col = 0; col < ColumnCount; col++)
            {
                var index = row * ColumnCount + col;
                if (OccupyList[index])
                {
                    occupyPositionList.Add(new Vector2Int(row, col));
                }
            }
        }

        return new GridPuzzlePieceStaticData(name, BuildSprite(), occupyPositionList.ToArray(), RowCount, ColumnCount);
    }

    private static readonly int pixelPerBlock = 1;
    public Sprite BuildSprite()
    {
        var texture = new Texture2D(ColumnCount * pixelPerBlock, RowCount * pixelPerBlock);
        texture.filterMode = FilterMode.Point; // 선명한 픽셀 렌더링

        // 각 픽셀을 순회하며 색 설정
        for (int row = 0; row < RowCount; row++)
        {
            for (int col = 0; col < ColumnCount; col++)
            {
                var occupyIndex = row * ColumnCount + col;
                var occupy = OccupyList[occupyIndex];
                var color = occupy ? Color.white : Color.clear;

                // 블록 하나를 pixelPerBlock 크기로 칠함
                for (int py = 0; py < pixelPerBlock; py++)
                {
                    for (int px = 0; px < pixelPerBlock; px++)
                    {
                        texture.SetPixel(col * pixelPerBlock + px, (RowCount - 1 - row) * pixelPerBlock + py, color);
                    }
                }
            }
        }

        // 텍스처 적용
        texture.Apply();

        // 스프라이트로 변환하여 Image에 적용
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
