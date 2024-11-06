using System.Collections.Generic;
using UnityEngine;

public class GridPuzzleUI : MonoBehaviour, IGridPuzzleUI
{
    public GridPuzzlePiece HoldingPiece { get; private set; }

    [SerializeField]
    private GridPuzzleBoardStaticData boardData;

    [SerializeField]
    private List<GridPuzzlePieceStaticData> pieceDataList = new List<GridPuzzlePieceStaticData>();

    [SerializeField]
    private float tileSize = 100f;

    [SerializeField]
    private GridPuzzleBoardControl boardControl;

    [SerializeField]
    private GridPuzzlePieceListControl pieceListControl;

    [SerializeField]
    private GridPuzzlePiecePlacePreviewControl piecePreviewControl;

    private GridPuzzleGame puzzleGame;

    public float TileSize => tileSize;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        puzzleGame = new GridPuzzleGame(boardData.RowCount, boardData.ColumnCount, pieceDataList);
        boardControl.Initialize(puzzleGame.BuildBoardSnapshot(), tileSize, this);
        pieceListControl.Initialize(puzzleGame.Pieces, this);
    }

    private void Update()
    {
        if (!IsHoldingPiece())
        {
            return;
        }

        Vector2 mousePosition = Input.mousePosition;
        piecePreviewControl.transform.position = mousePosition;
    }

    public bool IsHoldingPiece()
    {
        return HoldingPiece != null;
    }

    public void SetHoldingPiece(GridPuzzlePiece piece)
    {
        HoldingPiece = piece;

        piecePreviewControl.Show(piece, tileSize);
        Debug.Log($"piece held");
    }

    public void PlacePiece(Vector2Int tilePosition)
    {
        if (!IsHoldingPiece())
        {
            return;
        }

        if (!boardControl.PuzzleBoard.CanPlace(HoldingPiece, tilePosition))
        {
            return;
        }

        puzzleGame.Place(HoldingPiece, tilePosition);

        piecePreviewControl.Hide();

        boardControl.UpdateBoard(puzzleGame.BuildBoardSnapshot());

        pieceListControl.Apply(GetHidePieces());

        HoldingPiece = null;
    }

    public void DisplacePiece(Vector2Int tilePosition)
    {
    }

    private HashSet<int> GetHidePieces()
    {
        var result = new HashSet<int>();
        foreach (var id in puzzleGame.PlacedPieces.Keys)
        {
            result.Add(id);
        }

        if (HoldingPiece != null)
        {
            result.Add(HoldingPiece.InstanceId);
        }

        return result;
    }
}
