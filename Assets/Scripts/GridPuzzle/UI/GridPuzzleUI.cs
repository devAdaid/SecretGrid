using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridPuzzleUI : MonoSingleton<GridPuzzleUI>, IPointerClickHandler
{
    public GridPuzzlePiece HoldingPiece { get; private set; }

    [SerializeField]
    private GridPuzzleGameScriptableData testGameData;

    [SerializeField]
    private float tileSize = 100f;

    [SerializeField]
    private GridPuzzleBoardControl boardControl;

    [SerializeField]
    private GridPuzzlePieceListControl pieceListControl;

    [SerializeField]
    private GridPuzzlePiecePlacePreviewControl piecePreviewControl;

    [SerializeField]
    private Button nextButton;

    private GridPuzzleGame puzzleGame;

    private void Awake()
    {
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    public void SetHoldingPiece(GridPuzzlePiece piece)
    {
        HoldingPiece = piece;
        UpdateUI();
    }

    private void Start()
    {
        InitializeStage(testGameData.ToStaticData());
        UpdateUI();
    }

    private void Update()
    {
        // ������ �ǽ��� ���콺�� ���󰡵���
        if (IsHoldingPiece())
        {
            var mousePosition = Input.mousePosition;
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
            var leftUpAdjustedPosition = localPoint + GridPuzzleUIUtility.GetCenterToLeftUpOffset(HoldingPiece, tileSize);
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

        // ���������� ȸ��
        HoldingPiece.Rotate90();
        piecePreviewControl.UpdateRoate();
    }

    private void OnNextButtonClicked()
    {
        // TODO: ���� �������� �ʱ�ȭ
        InitializeStage(testGameData.ToStaticData());
        UpdateUI();
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

    private void InitializeStage(GridPuzzleGameStaticData gameData)
    {
        puzzleGame = new GridPuzzleGame(gameData);
        boardControl.Initialize(puzzleGame.BuildBoardSnapshot(), puzzleGame.Pieces, tileSize);
        pieceListControl.Initialize(puzzleGame.Pieces, tileSize);
    }

    private void UpdateUI()
    {
        if (IsHoldingPiece())
        {
            piecePreviewControl.Show(HoldingPiece, tileSize);
        }
        else
        {
            piecePreviewControl.Hide();
        }

        boardControl.Apply(puzzleGame.BuildBoardSnapshot(), puzzleGame.PlacedPiecePositionMap);

        pieceListControl.Apply(GetHidePieces());

        nextButton.gameObject.SetActive(puzzleGame.IsCleared());
    }

    private bool IsHoldingPiece()
    {
        return HoldingPiece != null;
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
