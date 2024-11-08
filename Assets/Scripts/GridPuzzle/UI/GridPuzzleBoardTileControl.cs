using UnityEngine;
using UnityEngine.UI;

public struct GridPuzzleBoardTileControlInitializeParameter : ISpawnableObjectInitializeParameter
{
    public readonly float TileSize;

    public GridPuzzleBoardTileControlInitializeParameter(float tileSize)
    {
        TileSize = tileSize;
    }
}

public class GridPuzzleBoardTileControl : MonoBehaviour, ISpawnableObject
{
    [SerializeField]
    private Image tileImage;

    [SerializeField]
    private RectTransform rectTransform;

    public void Initialize(ISpawnableObjectInitializeParameter parameter)
    {
        if (parameter is not GridPuzzleBoardTileControlInitializeParameter param)
        {
            return;
        }

        rectTransform.sizeDelta = new Vector2(param.TileSize, param.TileSize);
    }

    public void Despawn()
    {
        ObjectPoolHolder.I.BoardTilePool.Despawn(this);
    }

    public void SetOccupy(bool occupy)
    {
        //tileImage.color = occupy ? Color.red : Color.white;
    }

    public void SetPreview(bool preview)
    {
        var color = preview ? Color.yellow : tileImage.color;
        color.a = preview ? 0.25f : 1f;
        tileImage.color = color;
    }
}
