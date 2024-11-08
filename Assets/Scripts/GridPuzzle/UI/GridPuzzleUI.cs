using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridPuzzleUI : MonoSingleton<GridPuzzleUI>, IPointerClickHandler
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

    public void SetHoldingPiece(GridPuzzlePiece piece)
    {
        HoldingPiece = piece;
        UpdateUI();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        puzzleGame = new GridPuzzleGame(boardData.RowCount, boardData.ColumnCount, pieceDataList);
        boardControl.Initialize(puzzleGame.BuildBoardSnapshot(), puzzleGame.Pieces, tileSize);
        pieceListControl.Initialize(puzzleGame.Pieces, tileSize);
    }

    private void Update()
    {
        // 프리뷰 피스가 마우스를 따라가도록
        if (IsHoldingPiece())
        {
            Vector2 mousePosition = Input.mousePosition;
            piecePreviewControl.transform.position = mousePosition;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick(eventData.position);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    private void OnLeftClick(Vector2 mousePosition)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            boardControl.RectTransform,
            mousePosition,
            null,
            out Vector2 localPoint
        ))
        {
            HoldingPiece = null;
            UpdateUI();
            return;
        }

        if (IsHoldingPiece())
        {
            var tilePosition = boardControl.CalculateTilePosition(localPoint);
            var leftUpAdjustedPosition = localPoint + GridPuzzleUIUtility.GetCenterToLeftUpOffset(HoldingPiece, TileSize);
            var placeTilePosition = boardControl.CalculateTilePosition(leftUpAdjustedPosition);

            if (boardControl.PuzzleBoard.CanPlace(HoldingPiece, placeTilePosition))
            {
                PlacePiece(placeTilePosition);
                return;
            }

            if (!boardControl.PuzzleBoard.IsValidPosition(tilePosition))
            {
                HoldingPiece = null;
                UpdateUI();
            }
        }
        else
        {
            var tilePosition = boardControl.CalculateTilePosition(localPoint);
            if (boardControl.PuzzleBoard.TryGetTile(tilePosition, out var tile) && tile.OccupyingPieceId != 0)
            {
                DisplacePiece(tilePosition);
                return;
            }
        }
    }

    private void OnRightClick()
    {
        if (!IsHoldingPiece())
        {
            return;
        }

        // 오른쪽으로 회전
        HoldingPiece.Rotate90();
        piecePreviewControl.UpdateRoate();
    }


    private bool IsHoldingPiece()
    {
        return HoldingPiece != null;
    }

    private void PlacePiece(Vector2Int tilePosition)
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
        HoldingPiece = null;
        UpdateUI();

    }

    private void DisplacePiece(Vector2Int tilePosition)
    {
        if (!boardControl.PuzzleBoard.TryGetTile(tilePosition, out var tile))
        {
            return;
        }

        if (!puzzleGame.PieceMap.TryGetValue(tile.OccupyingPieceId, out var piece))
        {
            return;
        }

        puzzleGame.Displace(tile.OccupyingPieceId);
        HoldingPiece = piece;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (IsHoldingPiece())
        {
            piecePreviewControl.Show(HoldingPiece, TileSize);
        }
        else
        {
            piecePreviewControl.Hide();
        }

        boardControl.Apply(puzzleGame.BuildBoardSnapshot(), puzzleGame.PlacedPiecePositionMap);

        pieceListControl.Apply(GetHidePieces());
    }

    private HashSet<int> GetHidePieces()
    {
        var result = new HashSet<int>();
        foreach (var id in puzzleGame.PlacedPiecePositionMap.Keys)
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
