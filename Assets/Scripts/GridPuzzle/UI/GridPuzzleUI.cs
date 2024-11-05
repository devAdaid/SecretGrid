using System.Collections.Generic;
using UnityEngine;

public class GridPuzzleUI : MonoBehaviour, IGridPuzzleUI
{
    [SerializeField]
    private float tileSize = 100f;

    [SerializeField]
    private GridPuzzleBoardControl boardControl;

    [SerializeField]
    private List<GridPuzzlePieceControl> pieces = new List<GridPuzzlePieceControl>();

    [SerializeField]
    private GridPuzzlePiecePlacePreviewControl piecePreviewPrefab;

    private GridPuzzlePieceControl holdingPiece;
    private GridPuzzlePiecePlacePreviewControl piecePreviewControl;

    private GridPuzzleGame puzzleGame;

    public GridPuzzlePieceControl HoldingPiece => holdingPiece;

    public float TileSize => tileSize;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        puzzleGame = new GridPuzzleGame(3, 5);
        boardControl.Initialize(puzzleGame.BuildBoardSnapshot(), tileSize, this);
        foreach (var piece in pieces)
        {
            piece.Initialize(this);
        }
    }

    private void Update()
    {
        if (piecePreviewControl == null)
        {
            return;
        }

        Vector2 mousePosition = Input.mousePosition;
        piecePreviewControl.transform.position = mousePosition;
    }

    public bool IsHoldingPiece()
    {
        return holdingPiece != null;
    }

    public void SetHoldingPiece(GridPuzzlePieceControl piece)
    {
        holdingPiece = piece;

        piecePreviewControl = Instantiate(piecePreviewPrefab, transform);
        piecePreviewControl.Initialize(holdingPiece.Piece, tileSize);
        Debug.Log($"piece held");
    }

    public void PlaceTile(Vector2Int tilePosition)
    {
        if (!boardControl.IsTilePositionValid(tilePosition))
        {
            return;
        }

        if (holdingPiece != null)
        {
            puzzleGame.Place(holdingPiece.Piece, tilePosition);

            piecePreviewControl.Hide();
            Destroy(piecePreviewControl);

            boardControl.UpdateBoard(puzzleGame.BuildBoardSnapshot());

            holdingPiece = null;
        }
    }
}
