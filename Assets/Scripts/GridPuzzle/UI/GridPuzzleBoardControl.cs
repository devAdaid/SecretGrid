using System.Collections.Generic;
using UnityEngine;

public class GridPuzzleBoardControl : MonoBehaviour
{
    [SerializeField]
    private Transform tileRoot;

    [SerializeField]
    private Transform pieceRoot;

    [field: SerializeField]
    public RectTransform RectTransform { get; private set; }

    public GridPuzzleBoard PuzzleBoard { get; private set; }
    private float tileSize;
    private GridPuzzleBoardTileControl[,] tileArray;

    private Dictionary<int, GridPuzzlePlacePieceControl> placePieceMap = new Dictionary<int, GridPuzzlePlacePieceControl>();

    public void Initialize(GridPuzzleBoard board, List<GridPuzzlePiece> pieces, float tileSize)
    {
        Clear();

        this.PuzzleBoard = board;
        this.tileSize = tileSize;

        RectTransform.sizeDelta = new Vector2(tileSize * board.ColumnCount, tileSize * board.RowCount);

        SpawnTiles();
        PreSpawnPieces(pieces);
    }

    public void Apply(GridPuzzleBoard board, Dictionary<int, Vector2Int> placedPiecePositionMap)
    {
        PuzzleBoard = board;
        for (int row = 0; row < PuzzleBoard.RowCount; row++)
        {
            for (int column = 0; column < PuzzleBoard.ColumnCount; column++)
            {
                tileArray[row, column].SetOccupy(board.TileArray[row, column].IsOccupied);
            }
        }

        foreach (var pieceControl in placePieceMap.Values)
        {
            pieceControl.SetActive(placedPiecePositionMap.ContainsKey(pieceControl.Piece.InstanceId));

            if (placedPiecePositionMap.TryGetValue(pieceControl.Piece.InstanceId, out var placedPosition))
            {
                var localPos = tileArray[placedPosition.x, placedPosition.y].transform.localPosition;
                var offset = pieceControl.GetLeftUpToCenterOffset();
                localPos.x += offset.x;
                localPos.y += offset.y;
                pieceControl.transform.localPosition = localPos;
            }
        }
    }

    private void Clear()
    {
        if (tileArray != null)
        {
            foreach (var tile in tileArray)
            {
                tile.Despawn();
            }
            tileArray = null;
        }

        var pieces = placePieceMap.Values;
        foreach (var piece in pieces)
        {
            piece.Despawn();
        }
        placePieceMap.Clear();
    }

    private void SpawnTiles()
    {
        tileArray = new GridPuzzleBoardTileControl[PuzzleBoard.RowCount, PuzzleBoard.ColumnCount];

        var startX = -((PuzzleBoard.ColumnCount - 1) * tileSize) / 2;
        var startY = ((PuzzleBoard.RowCount - 1) * tileSize) / 2;
        for (int row = 0; row < PuzzleBoard.RowCount; row++)
        {
            for (int column = 0; column < PuzzleBoard.ColumnCount; column++)
            {
                var tile = CreateTile(row, column, tileSize);
                var posX = startX + column * tileSize;
                var posY = startY - row * tileSize;
                tile.transform.localPosition = new Vector3(posX, posY, 0);
                tileArray[row, column] = tile;
            }
        }
    }

    private void PreSpawnPieces(List<GridPuzzlePiece> pieces)
    {
        foreach (var piece in pieces)
        {
            var pieceControl = ObjectPoolHolder.I.PlacePiecePool.Spawn(tileRoot, new GridPuzzlePlacePieceControlInitializeParameter(piece, tileSize));
            pieceControl.gameObject.SetActive(false);
            placePieceMap.Add(piece.InstanceId, pieceControl);
        }
    }

    private GridPuzzleBoardTileControl CreateTile(int row, int column, float tileSize)
    {
        var tileObject = ObjectPoolHolder.I.BoardTilePool.Spawn(tileRoot, new GridPuzzleBoardTileControlInitializeParameter(tileSize));
        tileObject.name = $"Tile_{row}_{column}";
        return tileObject;
    }

    /*
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

        if (puzzleUI.HoldingPiece != null)
        {
            localPoint += GridPuzzleUIUtility.GetLeftUpOffset(puzzleUI.HoldingPiece, puzzleUI.TileSize);

            Vector2Int tilePosition = CalculateTilePosition(localPoint);
            if (PuzzleBoard.IsValidPosition(tilePosition))
            {
                puzzleUI.PlacePiece(tilePosition);
                Debug.Log($"Tile place to {tilePosition}");
            }
        }
        else
        {
            Vector2Int tilePosition = CalculateTilePosition(localPoint);
            if (PuzzleBoard.IsOccupiedByPiece(tilePosition, out _))
            {
                puzzleUI.DisplacePiece(tilePosition);
            }
        }

    }
    */
    public Vector2Int CalculateTilePosition(Vector2 localPoint)
    {
        float offsetX = RectTransform.rect.width / 2f;
        float offsetY = RectTransform.rect.height / 2f;

        int col = Mathf.FloorToInt((localPoint.x + offsetX) / tileSize);
        int row = Mathf.FloorToInt((offsetY - localPoint.y) / tileSize);

        return new Vector2Int(row, col);
    }
}
