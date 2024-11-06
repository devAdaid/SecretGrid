using UnityEngine;

[CreateAssetMenu(fileName = "GridPuzzlePieceData", menuName = "Scriptable Objects/GridPuzzlePieceData")]
public class GridPuzzlePieceStaticData : ScriptableObject
{
    // TODO: https://www.sunnyvalleystudio.com/blog/unity-2d-sprite-preview-inspector-custom-editor
    [field: SerializeField]
    public Sprite Sprite;

    [field: SerializeField]
    public Vector2Int[] OccupyPositions;
}
