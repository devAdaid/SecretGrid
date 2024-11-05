using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridPuzzleBoardControl : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;

    [SerializeField]
    private GridPuzzleBoardTileControl tilePrefab;

    public RectTransform RectTransform { get; private set; }

    public GridPuzzleBoard PuzzleBoard { get; private set; }
    private float tileSize;
    private GridPuzzleBoardTileControl[,] tileArray;
    private IGridPuzzleUI puzzleUI;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(GridPuzzleBoard board, float tileSize, IGridPuzzleUI puzzleUI)
    {
        this.PuzzleBoard = board;
        this.tileSize = tileSize;
        this.puzzleUI = puzzleUI;

        gridLayoutGroup.cellSize = new Vector2(tileSize, tileSize);

        SpawnTiles();
    }

    public void UpdateBoard(GridPuzzleBoard board)
    {
        for (int row = 0; row < PuzzleBoard.RowCount; row++)
        {
            for (int column = 0; column < PuzzleBoard.ColumnCount; column++)
            {
                tileArray[row, column].SetOccupy(board.TileArray[row, column].IsOccupied);
            }
        }
    }

    private void SpawnTiles()
    {
        this.tileArray = new GridPuzzleBoardTileControl[PuzzleBoard.RowCount, PuzzleBoard.ColumnCount];
        for (int row = 0; row < PuzzleBoard.RowCount; row++)
        {
            for (int column = 0; column < PuzzleBoard.ColumnCount; column++)
            {
                var tile = CreateTile(row, column);
                tileArray[row, column] = tile;
            }
        }
    }

    private GridPuzzleBoardTileControl CreateTile(int row, int column)
    {
        var tileObject = Instantiate(tilePrefab, gridLayoutGroup.transform);
        tileObject.name = $"Tile_{row}_{column}";
        tileObject.Initialize(new Vector2Int(column, row));
        return tileObject;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position, eventData.pressEventCamera, out var localPoint))
        {
            return;
        }

        if (puzzleUI.HoldingPiece == null)
        {
            return;
        }

        localPoint += GridPuzzleUIUtility.GetLeftUpOffset(puzzleUI.HoldingPiece.Piece, puzzleUI.TileSize);

        Vector2Int tilePosition = CalculateTilePosition(localPoint);
        if (IsTilePositionValid(tilePosition))
        {
            OnTileClicked(tilePosition);
        }
    }

    private Vector2Int CalculateTilePosition(Vector2 localPoint)
    {
        float offsetX = RectTransform.rect.width / 2f;
        float offsetY = RectTransform.rect.height / 2f;

        int col = Mathf.FloorToInt((localPoint.x + offsetX) / tileSize);
        int row = Mathf.FloorToInt((offsetY - localPoint.y) / tileSize);

        return new Vector2Int(row, col);
    }

    public bool IsTilePositionValid(Vector2Int tilePosition)
    {
        return tilePosition.x >= 0 && tilePosition.y >= 0 && tilePosition.x < PuzzleBoard.RowCount && tilePosition.y < PuzzleBoard.ColumnCount;
    }

    private void OnTileClicked(Vector2Int tilePosition)
    {
        Debug.Log($"Tile {tilePosition} clicked");
        puzzleUI.PlaceTile(tilePosition);
    }
}
