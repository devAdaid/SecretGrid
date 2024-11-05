using UnityEngine;
using UnityEngine.UI;

public class GridPuzzlePiecePlacePreviewControl : MonoBehaviour
{
    [SerializeField]
    private Image previewImage;

    [SerializeField]
    private RectTransform tileRoot;

    [SerializeField]
    private GridPuzzleBoardTileControl tilePrefab;

    private RectTransform rectTransform;

    private bool show;
    private GridPuzzlePiece holdingPiece;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(GridPuzzlePiece piece, float tileSize)
    {
        holdingPiece = piece;
        gameObject.SetActive(true);
        rectTransform.sizeDelta = new Vector2(piece.ColumnSize * tileSize, piece.RowSize * tileSize);

        var leftUpPosition = GridPuzzleUIUtility.GetLeftUpOffset(piece, tileSize);
        foreach (var occupyPosition in holdingPiece.OccupyPositions)
        {
            var tile = Instantiate(tilePrefab, tileRoot);
            var positionColumn = occupyPosition.y;
            var positionRow = occupyPosition.x;
            var newPosition = leftUpPosition + new Vector2(positionColumn * tileSize, -positionRow * tileSize);
            tile.transform.localPosition = newPosition;
            tile.SetSize(tileSize);
            tile.SetPreview(true);
        }
    }

    public void Hide()
    {
        holdingPiece = null;
        gameObject.SetActive(false);
    }
}
