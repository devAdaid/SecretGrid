using System;
public class GridPuzzleGamePieceStaticData
{
    public GridPuzzlePieceStaticData Piece;
    public GridPuzzlePiecePlaceInfo PlaceInfo;

    public GridPuzzleGamePieceStaticData(GridPuzzlePieceStaticData piece, GridPuzzlePiecePlaceInfo placeInfo)
    {
        Piece = piece;
        PlaceInfo = placeInfo;
    }
}

[Serializable]
public class GridPuzzleGamePieceData
{
    public GridPuzzlePieceScriptableData Piece;
    public GridPuzzlePiecePlaceInfo PlaceInfo;
}