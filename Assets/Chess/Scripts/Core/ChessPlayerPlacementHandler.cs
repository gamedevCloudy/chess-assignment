using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Chess.Scripts.Core
{
    public class ChessPlayerPlacementHandler : MonoBehaviour
    {
        private bool _isHighlighting = false;
        private Piece _highlightedPiece;


        [SerializeField] private LayerMask _pieceLayer2D;
        [SerializeField] private GameObject _highlighter;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ChessBoardPlacementHandler.Instance.ClearHighlights();

                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, _pieceLayer2D);

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent<Piece>(out Piece piece))
                    {
                        if (piece.GetPieceColor == Piece.PieceColor.White)
                        {

                            HighlightSelectedPiece(piece);
                            SuggestMove(piece);
                        }
                    }
                }
                else if (_isHighlighting)
                {

                    ClearSelected();
                    ChessBoardPlacementHandler.Instance.ClearHighlights();
                }
            }
        }

        private void HighlightSelectedPiece(Piece piece)
        {
            if (_isHighlighting)
            {
                ClearSelected();
            }
            _isHighlighting = true;
            _highlightedPiece = piece;
            _highlighter.SetActive(true);
            _highlighter.transform.position = piece.transform.position;
        }


        private void ClearSelected()
        {
            _isHighlighting = false;
            _highlightedPiece = null;
            _highlighter.SetActive(false);
        }


        private void SuggestMove(Piece piece)
        {

            Vector2Int pos = piece.GetTilePos;

            switch (piece.GetPieceType)
            {
                case Piece.PieceType.Pawn:
                    //1 square -> up
                    GetPawnMovement(pos); 

                    break;
                case Piece.PieceType.Rook:
                   
                    GetStraightMovement(pos); 
                    break;

                case Piece.PieceType.Bishop:
                    GetDiagonalMovement(pos); 
                    //all squares -> diagonally
                    break;
                case Piece.PieceType.Queen:
                    //all squares -> up, down, left, right
                    //all squares -> diagonally

                    GetStraightMovement(pos); 
                    GetDiagonalMovement(pos); 

                    break;
                case Piece.PieceType.Knight:
                    //dhai -> all directions -> front left and right, back left and right
                    GetKnightMoves(pos); 
                    break;
                case Piece.PieceType.King:
                    //1 square -> up, down, left, right, diagonal
                    GetKingMoves(pos); 
                    break;

            }
        }


        private void GetPawnMovement(Vector2Int pos)
        {
            if (!IsOccupied(pos.x + 1, pos.y) && IsWithinBoard(pos.x+1, pos.y))
            {
                HighlightOnBoard(pos.x + 1, pos.y);
            }
        }
        private void GetStraightMovement(Vector2Int pos)
        {
            // Loop through all four directions
            Vector2Int[] directions = new Vector2Int [] {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
            foreach (var direction in directions)
            {
                for (int i = 1; i < 8; i++)
                {
                    int x = pos.x + direction.x * i;
                    int y = pos.y + direction.y * i;

                    // Check if the square is within the board and not occupied
                    if (IsWithinBoard(x, y) && !IsOccupied(x, y))
                    {
                        HighlightOnBoard(x, y);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        
        private void GetDiagonalMovement(Vector2Int pos)
        {
            // Loop through both directions of the diagonal movement
            for (int direction = -1; direction <= 1; direction += 2)
            {
                int yOffset = pos.y + direction;
                for (int xOffset = 1; xOffset < 8; xOffset++)
                {
                    int x = pos.x + xOffset * direction;
                    int y = yOffset;

                    // Check if the square is within the board and not occupied
                    if (IsWithinBoard(x, y) && !IsOccupied(x, y))
                    {
                        HighlightOnBoard(x, y);
                    }
                    else
                    {
                        break;
                    }

                    // Update yOffset for next iteration
                    yOffset += direction;
                }
            }
        }

        private void GetKnightMoves(Vector2Int pos)
        {
            int[] xOffsets = { 2, 2, -2, -2, 1, -1, 1, -1 };
            int[] yOffsets = { 1, -1, 1, -1, 2, 2, -2, -2 };

            for (int i = 0; i < 8; i++)
            {
                int x = pos.x + xOffsets[i];
                int y = pos.y + yOffsets[i];

                if (IsWithinBoard(x, y) && !IsOccupied(x, y))
                {
                    HighlightOnBoard(x, y);
                }
            }
        }

       private void GetKingMoves(Vector2Int pos)
        {
            int[] xOffsets = { 1, 1, 1, 0, 0, -1, -1, -1 };
            int[] yOffsets = { 1, 0, -1, 1, -1, 1, 0, -1 };

            for (int i = 0; i < 8; i++)
            {
                int x = pos.x + xOffsets[i];
                int y = pos.y + yOffsets[i];

                if (IsWithinBoard(x, y) && !IsOccupied(x, y))
                {
                    HighlightOnBoard(x, y);
                }
            }
        }


        private bool IsWithinBoard(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }
        private bool IsOccupied(int row, int column)
        {
            return ChessBoardPlacementHandler.Instance.isTileOccupied(row, column); 
        }

        private void HighlightOnBoard(int row, int column){
            ChessBoardPlacementHandler.Instance.Highlight(row, column); 
        }
    }
}