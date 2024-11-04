using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridPuzzlePieceControl : MonoBehaviour, IPointerClickHandler
{
    public List<Vector2Int> TempShape;

    private RectTransform rectTransform;
    private IGridPuzzleUI puzzleUI;
    public GridPuzzlePiece Piece { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        var occupyPositions = TempShape.ToArray();
        Piece = new GridPuzzlePiece(GridPuzzleRotateType.Rotate0, occupyPositions);
    }

    public void Initialize(IGridPuzzleUI puzzleUI)
    {
        this.puzzleUI = puzzleUI;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        puzzleUI.SetHoldingPiece(this);
    }
}
