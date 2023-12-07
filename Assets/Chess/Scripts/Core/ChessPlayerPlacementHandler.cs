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
        [SerializeField] private GameObject _selectedHighlight;

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

                            HighlightPiece(piece);
                            SuggestMove(piece);
                        }
                    }
                }
                else if (_isHighlighting)
                {

                    ClearHighlight();
                    ChessBoardPlacementHandler.Instance.ClearHighlights();
                }
            }
        }

        private void HighlightPiece(Piece piece)
        {
            if (_isHighlighting)
            {
                ClearHighlight();
            }
            _isHighlighting = true;
            _highlightedPiece = piece;
            _selectedHighlight.SetActive(true);
            _selectedHighlight.transform.position = piece.transform.position;
        }


        private void ClearHighlight()
        {
            _isHighlighting = false;
            _highlightedPiece = null;
            _selectedHighlight.SetActive(false);
        }


        private void SuggestMove(Piece piece)
        {

            Vector2Int pos = piece.GetTilePos;

            switch (piece.GetPieceType)
            {
                case Piece.PieceType.Pawn:
                    //1 square -> up
                   
                    if (piece.GetPieceColor == Piece.PieceColor.White)
                    {

                        if (ChessBoardPlacementHandler.Instance.isTileOccupied(pos.x + 1, pos.y))
                        {
                            ChessBoardPlacementHandler.Instance.Highlight(pos.x + 1, pos.y);
                        }
                    }
                    else
                    {
                        if (ChessBoardPlacementHandler.Instance.isTileOccupied(pos.x - 1, pos.y))
                        {
                            ChessBoardPlacementHandler.Instance.Highlight(pos.x - 1, pos.y);
                        }
                    }

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

        private void GetStraightMovement(Vector2Int pos){
            //right
            for (int i = pos.x + 1; i < 8; i++)
            {
                bool check = ChessBoardPlacementHandler.Instance.isTileOccupied(i, pos.y);
                if(check) break; 
                ChessBoardPlacementHandler.Instance.Highlight(i, pos.y);
            }
            //left
            for (int i = pos.x - 1; i >= 0; i--)
            {
                bool check = ChessBoardPlacementHandler.Instance.isTileOccupied(i, pos.y);
                if(check) break; 
                ChessBoardPlacementHandler.Instance.Highlight(i, pos.y);
            }
            //up
            for (int i = pos.y + 1; i < 8; i++)
            {
                bool check = ChessBoardPlacementHandler.Instance.isTileOccupied(pos.x, i);
                if(check) break; 
                ChessBoardPlacementHandler.Instance.Highlight(pos.x, i);

            }
            //down
            for (int i = pos.y - 1; i >= 0; i--)
            {
                bool check = ChessBoardPlacementHandler.Instance.isTileOccupied(pos.x, i);
                if(check) break; 
                ChessBoardPlacementHandler.Instance.Highlight(pos.x, i);
            }

        }
        private void GetDiagonalMovement(Vector2Int pos){
            // //right
            int j = pos.y+1; 
            int k = pos.y-1; 

            for (int i = pos.x + 1; i < 8; i++)
            {   
                if(j <=7)
                {
                    bool check = ChessBoardPlacementHandler.Instance.isTileOccupied(i, j); 
                    if(check) break; 
                    ChessBoardPlacementHandler.Instance.Highlight(i,j); 
                }      
                j+=1; 
            }
            for (int i = pos.x + 1; i < 8; i++)
            {   
                if(k >=0)
                {
                    bool check = ChessBoardPlacementHandler.Instance.isTileOccupied(i, k); 
                    if(check) break; 
                    ChessBoardPlacementHandler.Instance.Highlight(i,k); 
                }      
                k-=1; 
            }

            j = pos.y+1; 
            for (int i = pos.x - 1; i >= 0; i--)
            {   
                if(j <=7)
                {
                    bool check = ChessBoardPlacementHandler.Instance.isTileOccupied(i, j); 
                    if(check) break; 
                    ChessBoardPlacementHandler.Instance.Highlight(i,j); 
                }      
                j+=1; 
            }

            k = pos.y-1; 
            for (int i = pos.x - 1; i >= 0; i--)
            {   
                if(k >=0)
                {
                    bool check = ChessBoardPlacementHandler.Instance.isTileOccupied(i, k); 
                    if(check) break; 
                    ChessBoardPlacementHandler.Instance.Highlight(i,k); 
                }      
                k-=1; 
            }
            
        }
    
        private bool IsWithinBoard(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }

        private void GetKnightMoves(Vector2Int pos)
        {
            int[] xOffsets = { 2, 2, -2, -2, 1, -1, 1, -1 };
            int[] yOffsets = { 1, -1, 1, -1, 2, 2, -2, -2 };

            for (int i = 0; i < 8; i++)
            {
                int x = pos.x + xOffsets[i];
                int y = pos.y + yOffsets[i];

                if (IsWithinBoard(x, y) && !ChessBoardPlacementHandler.Instance.isTileOccupied(x, y))
                {
                    ChessBoardPlacementHandler.Instance.Highlight(x, y);
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

                if (IsWithinBoard(x, y) && !ChessBoardPlacementHandler.Instance.isTileOccupied(x, y))
                {
                    ChessBoardPlacementHandler.Instance.Highlight(x, y);
                }
            }
        }



        private bool isValidMove(int row, int column)
        {
            // Implement logic to check if the move is valid
            // based on game rules and board state
            // ...
            if (row < 0 || row >= 8) return false;
            if (column < 0 || column >= 8) return false;

            GameObject g = ChessBoardPlacementHandler.Instance.GetTile(row, column);
            if (g == null) return false;
            // if(g.transform.GetChild(0) != null) return false; 
            // Replace the following with your actual check
            return true;
        }
    }
}
