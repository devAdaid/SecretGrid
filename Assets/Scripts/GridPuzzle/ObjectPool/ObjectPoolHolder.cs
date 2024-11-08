using UnityEngine;

public class ObjectPoolHolder : MonoSingleton<ObjectPoolHolder>
{
    [field: SerializeField]
    public GridPuzzlePlacePieceControlPool PlacePiecePool;
    [field: SerializeField]
    public GridPuzzlePieceControlPool PiecePool;
    [field: SerializeField]
    public GridPuzzleBoardTileControlPool BoardTilePool;
}
