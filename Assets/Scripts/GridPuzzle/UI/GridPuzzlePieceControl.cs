public class GridPuzzlePieceControl : GridPuzzlePlacePieceControlBase, ISpawnableObject
{
    public void Initialize(ISpawnableObjectInitializeParameter parameter)
    {
        if (parameter is not GridPuzzlePieceControlInitializeParameter param)
        {
            return;
        }

        DoInitialize(param);
    }

    public void Despawn()
    {
        ObjectPoolHolder.I.PiecePool.Despawn(this);
    }

    public void SetActive(bool active)
    {
        transform.localEulerAngles = Piece.RotateState.ToEulerAngles();
        pieceImage.gameObject.SetActive(active);
    }

    public void OnClick()
    {
        GridPuzzleUI.I.SetHoldingPiece(Piece);
    }
}
