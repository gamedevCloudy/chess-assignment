using UnityEngine;
using Chess.Scripts.Core;

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
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, _pieceLayer2D);

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent<Piece>(out Piece piece))
                    {
                        HighlightPiece(piece);
                    }
                }
                else if (_isHighlighting)
                {
                    ClearHighlight();
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
    }
}
