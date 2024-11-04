using UnityEngine;

public interface IGridPuzzleUI
{
    bool IsHoldingPiece();
    // TODO: 인터페이스보다는 이벤트 콜백로 연결해야할듯?
    void SetHoldingPiece(GridPuzzlePieceControl piece);
    void OnTileClick(Vector2Int tilePosition);
}