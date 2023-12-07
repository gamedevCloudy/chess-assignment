using System.Collections;
using UnityEngine;

namespace Chess.Scripts.Core 
{
    public class SetupBoard : MonoBehaviour
    {
        // [Header("White Pieces")]

        [SerializeField] private GameObject whitePawn, whiteBishop, whiteKnight, whiteRook, whiteQueen, whiteKing;

        // [Header("Black Pieces")]
        [SerializeField] private GameObject blackPawn, blackBishop, blackKnight, blackRook, blackQueen, blackKing;

        private void Start()
        {
            PlacePieces(); 
        }

        private void PlacePieces()
        {
            // Place all pawns
            // for (int i = 0; i < 8; i++)
            // {
            //     InstantiatePiece(whitePawn, 1, i);
            //     InstantiatePiece(blackPawn, 6, i);
            // }

            // Place rooks
            // InstantiatePiece(whiteRook, 1, 1);
            // InstantiatePiece(whiteRook, 5, 5);
            // InstantiatePiece(blackRook, 1, 3);
            // InstantiatePiece(blackRook, 4, 1);

            // Place bishops
            // InstantiatePiece(whiteBishop, 0, 2);
            // InstantiatePiece(whiteBishop, 0, 5);
            // InstantiatePiece(blackBishop, 7, 2);
            // InstantiatePiece(blackBishop, 7, 5);

            // // Place knights
            // InstantiatePiece(whiteKnight, 0, 1);
            // InstantiatePiece(whiteKnight, 0, 6);
            // InstantiatePiece(blackKnight, 7, 1);
            // InstantiatePiece(blackKnight, 7, 6);
            

            // Place queens and kings
            // InstantiatePiece(whiteQueen, 0, 3);
            // InstantiatePiece(blackQueen, 7, 3);
            InstantiatePiece(whiteKing, 0, 4);
            InstantiatePiece(blackKing, 6, 4);
            InstantiatePiece(whiteKing, 1, 1);
            InstantiatePiece(whiteKing, 5, 5);
        }

        private void InstantiatePiece(GameObject piece, int row, int column)
        {
            GameObject newPiece = Instantiate(piece, ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position, Quaternion.identity);
            // newPiece.transform.SetParent(ChessBoardPlacementHandler.Instance.GetTile(row,column).transform); 
            Vector2Int tile = new Vector2Int(row, column); 
            newPiece.GetComponent<Piece>().SetTilePos(tile); 
        }
    }
}
