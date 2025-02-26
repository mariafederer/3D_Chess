using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Piece selectedPiece;
    private LogicManager logicManager;
    private List<Square> highlightedSquares = new List<Square>();

    void Start()
    {
        logicManager = Object.FindFirstObjectByType<LogicManager>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SelectPiece();
        }

    }

    void SelectPiece()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Piece piece = hit.transform.GetComponent<Piece>();
            if (piece != null)
            {
                if (selectedPiece == piece)
                {
                    UnhighlightLegalMoves();
                    selectedPiece = null;
                }
                else
                {
                    selectedPiece = piece;
                    HighlightLegalMoves(selectedPiece.GetLegalMoves());
                }
            }
            else
            {
                Square square = hit.transform.GetComponent<Square>();
                if (square != null)
                {
                    Vector2 squareCoordinates = new Vector2(square.transform.position.x, square.transform.position.z);
                    Piece pieceOnSquare = logicManager.boardMap[(int)squareCoordinates.x, (int)squareCoordinates.y];
                    if (pieceOnSquare != null)
                    {
                        if (selectedPiece == pieceOnSquare)
                        {
                            UnhighlightLegalMoves();
                            selectedPiece = null;
                        }
                        else
                        {
                            selectedPiece = pieceOnSquare;
                            HighlightLegalMoves(selectedPiece.GetLegalMoves());
                        }
                    }
                }
            }
        }
    }

    void HighlightLegalMoves(List<Vector2> legalMoves)
    {
        UnhighlightLegalMoves();

        foreach (Vector2 move in legalMoves)
        {
            Square square = GetSquareAtPosition(move);
            square.Highlight(Color.cyan);
            highlightedSquares.Add(square);
            Debug.Log("Highlight square at: " + move);
        }
    }

    void UnhighlightLegalMoves()
    {
        foreach (Square square in highlightedSquares)
        {
            square.Unhighlight();
        }
        highlightedSquares.Clear();
    }

    Square GetSquareAtPosition(Vector2 position)
    {
        Ray ray = new Ray(new Vector3(position.x, 1, position.y), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.transform.GetComponent<Square>();
        }
        return null;
    }
}
