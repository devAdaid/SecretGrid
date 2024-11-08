using System.Collections.Generic;
using UnityEngine;

public class GridPuzzlePieceListControl : MonoBehaviour
{
    [SerializeField]
    private GridPuzzlePieceControl piecePrefab;

    [SerializeField]
    private Transform pieceRoot;

    private List<GridPuzzlePieceControl> pieceControls = new List<GridPuzzlePieceControl>();

    public void Initialize(List<GridPuzzlePiece> pieces, float tileSize)
    {
        Clear();

        foreach (var piece in pieces)
        {
            var pieceControl = ObjectPoolHolder.I.PiecePool.Spawn(pieceRoot, new GridPuzzlePieceControlInitializeParameter(piece, tileSize));
            pieceControls.Add(pieceControl);
        }
    }

    public void Apply(HashSet<int> hidePieceIds)
    {
        foreach (var pieceControl in pieceControls)
        {
            pieceControl.SetActive(!hidePieceIds.Contains(pieceControl.Piece.InstanceId));
        }
    }

    private void Clear()
    {
        foreach (var piece in pieceControls)
        {
            piece.Despawn();
        }
        pieceControls.Clear();
    }
}
