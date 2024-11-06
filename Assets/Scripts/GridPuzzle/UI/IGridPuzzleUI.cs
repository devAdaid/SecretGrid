using UnityEngine;

public interface IGridPuzzleUI
{
    float TileSize { get; }
    GridPuzzlePiece HoldingPiece { get; }
    bool IsHoldingPiece();
    // TODO: 인터페이스보다는 이벤트 콜백로 연결해야할듯?
    void SetHoldingPiece(GridPuzzlePiece piece);
    void PlacePiece(Vector2Int tilePosition);
    void DisplacePiece(Vector2Int tilePosition);
}