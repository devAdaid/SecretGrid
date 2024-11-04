using System.Collections.Generic;
using UnityEngine;

public class GridPuzzleUI : MonoBehaviour, IGridPuzzleUI
{
    [SerializeField]
    private GridPuzzleBoardControl boardControl;

    [SerializeField]
    private List<GridPuzzlePieceControl> pieces = new List<GridPuzzlePieceControl>();

    private GridPuzzlePieceControl holdingPiece;

    private GridPuzzleGame puzzleGame;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        puzzleGame = new GridPuzzleGame(3, 5);
        boardControl.Initialize(puzzleGame.BuildBoardSnapshot(), 100, this);
        foreach (var piece in pieces)
        {
            piece.Initialize(this);
        }
    }

    public bool IsHoldingPiece()
    {
        return holdingPiece != null;
    }

    public void SetHoldingPiece(GridPuzzlePieceControl piece)
    {
        holdingPiece = piece;
        Debug.Log($"piece held");
    }

    public void OnTileClick(Vector2Int tilePosition)
    {
        if (!boardControl.IsTilePositionValid(tilePosition))
        {
            return;
        }

        if (holdingPiece != null)
        {
            puzzleGame.Place(holdingPiece.Piece, tilePosition);
            boardControl.UpdateBoard(puzzleGame.BuildBoardSnapshot());
            holdingPiece = null;
        }
    }
}
