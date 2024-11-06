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
        puzzleGame = new GridPuzzleGame(3, 5, new List<GridPuzzlePiece>());
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
        piecePreviewControl.Initialize(holdingPiece.Piece, holdingPiece.TempTileSprite, tileSize);
        Debug.Log($"piece held");
    }

    public void PlacePiece(Vector2Int tilePosition)
    {
        if (holdingPiece == null)
        {
            return;
        }

        if (!boardControl.PuzzleBoard.CanPlace(holdingPiece.Piece, tilePosition))
        {
            return;
        }

        puzzleGame.Place(holdingPiece.Piece, tilePosition);

        piecePreviewControl.Hide();
        Destroy(piecePreviewControl.gameObject);

        boardControl.UpdateBoard(puzzleGame.BuildBoardSnapshot());

        holdingPiece = null;
    }

    // TODO: 그냥 타일 데이터를 만들자. sprite 참조할 수 있게.
    public void DisplacePiece(Vector2Int tilePosition)
    {
        if (boardControl.PuzzleBoard.IsOccupiedByPiece(tilePosition, out var placedPiece))
        {
            puzzleGame.Displace(placedPiece);

            //holdingPiece = placedPiece;
            piecePreviewControl = Instantiate(piecePreviewPrefab, transform);
            piecePreviewControl.Initialize(holdingPiece.Piece, holdingPiece.TempTileSprite, tileSize);
        }
    }
}
