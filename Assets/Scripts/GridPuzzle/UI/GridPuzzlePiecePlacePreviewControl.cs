using UnityEngine;
using UnityEngine.UI;

public class GridPuzzlePiecePlacePreviewControl : MonoBehaviour
{
    [SerializeField]
    private Image previewImage;

    //[SerializeField]
    //private RectTransform tileRoot;

    //[SerializeField]
    //private GridPuzzleBoardTileControl tilePrefab;

    private GridPuzzlePiece holdingPiece;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show(GridPuzzlePiece piece, float tileSize)
    {
        holdingPiece = piece;
        gameObject.SetActive(true);
        previewImage.rectTransform.localEulerAngles = piece.RotateState.ToEulerAngles();
        previewImage.rectTransform.sizeDelta = new Vector2(piece.StaticData.ColumnCount * tileSize, piece.StaticData.RowCount * tileSize);
        previewImage.sprite = piece.StaticData.Sprite;

        //var leftUpPosition = GridPuzzleUIUtility.GetLeftUpOffset(piece, tileSize);
        //foreach (var occupyPosition in holdingPiece.OccupyPositions)
        //{
        //    var tile = Instantiate(tilePrefab, tileRoot);
        //    var positionColumn = occupyPosition.y;
        //    var positionRow = occupyPosition.x;
        //    var newPosition = leftUpPosition + new Vector2(positionColumn * tileSize, -positionRow * tileSize);
        //    tile.transform.localPosition = newPosition;
        //    tile.SetSize(tileSize);
        //    tile.SetPreview(true);
        //}
    }

    public void Hide()
    {
        holdingPiece = null;
        gameObject.SetActive(false);
    }

    public void UpdateRoate()
    {
        previewImage.rectTransform.localEulerAngles = holdingPiece.RotateState.ToEulerAngles();
    }
}
