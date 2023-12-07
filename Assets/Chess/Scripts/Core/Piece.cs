using UnityEngine;

namespace Chess.Scripts.Core
{
    [System.Serializable]
    public class Piece : MonoBehaviour
    {
       
        public enum PieceColor
        {
            Black,
            White
        }

        
        public enum PieceType
        {
            Pawn,
            Bishop,
            Rook,
            Knight,
            Queen,
            King
        }


        [SerializeField] private PieceType _pieceType;
        [SerializeField] private int _cost;
        [SerializeField] private PieceColor _color;

        public PieceType GetPieceType => _pieceType;
        public PieceColor GetPieceColor => _color;
        public int GetCost => _cost;
    }
}
