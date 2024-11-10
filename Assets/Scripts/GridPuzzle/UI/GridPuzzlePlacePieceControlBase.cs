using UnityEngine;
using UnityEngine.UI;
public class GridPuzzlePieceControlInitializeParameter : ISpawnableObjectInitializeParameter
{
    public readonly GridPuzzlePiece Piece;
    public readonly float TileSize;
    public readonly GridPuzzleGameStaticData GameStaticData;

    public GridPuzzlePieceControlInitializeParameter(GridPuzzlePiece piece, float tileSize, GridPuzzleGameStaticData boardStaticData)
    {
        Piece = piece;
        TileSize = tileSize;
        GameStaticData = boardStaticData;
    }
}

public abstract class GridPuzzlePlacePieceControlBase : MonoBehaviour
{
    public GridPuzzlePiece Piece { get; protected set; }

    [SerializeField]
    protected RectTransform rectTransform;

    [SerializeField]
    protected Image pieceImage;

    [SerializeField]
    protected Image pictureImage;

    protected float tileSize;

    protected void DoInitialize(GridPuzzlePieceControlInitializeParameter param)
    {
        Piece = param.Piece;
        tileSize = param.TileSize;

        pieceImage.sprite = param.Piece.StaticData.Sprite;
        pictureImage.sprite = param.GameStaticData.PictureSprite;

        pictureImage.rectTransform.localPosition = -GridPuzzleUIUtility.GetAnswerBoardCenterToPieceCenterOffset(
            param.Piece.AnswerPlaceInfo.Position,
            param.Piece,
            param.GameStaticData.BoardData,
            param.TileSize);

        transform.localEulerAngles = Piece.RotateState.ToEulerAngles();
        pictureImage.transform.localEulerAngles = Piece.AnswerPlaceInfo.RotateType.ToEulerAngles() * -1;

        rectTransform.sizeDelta = new Vector2(param.TileSize * param.Piece.StaticData.ColumnCount, param.TileSize * param.Piece.StaticData.RowCount);
        pictureImage.rectTransform.sizeDelta = new Vector2(param.TileSize * param.GameStaticData.BoardData.ColumnCount, param.TileSize * param.GameStaticData.BoardData.RowCount);
    }
}
