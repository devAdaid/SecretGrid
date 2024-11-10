public class GridPuzzlePiecePlacePreviewControl : GridPuzzlePlacePieceControlBase
{
    //[SerializeField]
    //private RectTransform tileRoot;

    //[SerializeField]
    //private GridPuzzleBoardTileControl tilePrefab;

    public void Show(GridPuzzlePieceControlInitializeParameter param)
    {
        gameObject.SetActive(true);
        DoInitialize(param);

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
        Piece = null;
        gameObject.SetActive(false);
    }

    public void UpdateRoate()
    {
        rectTransform.localEulerAngles = Piece.RotateState.ToEulerAngles();
    }
}
