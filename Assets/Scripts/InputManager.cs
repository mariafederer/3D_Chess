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
        if (Input.GetMouseButtonDown(0))
        {
            SelectPiece();
            MovePiece();
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
                if ((piece.IsWhite && logicManager.isWhiteTurn) || (!piece.IsWhite && !logicManager.isWhiteTurn))
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
                        if ((pieceOnSquare.IsWhite && logicManager.isWhiteTurn) || (!pieceOnSquare.IsWhite && !logicManager.isWhiteTurn))
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
    }

    void HighlightLegalMoves(List<Vector2> legalMoves)
    {
        UnhighlightLegalMoves();
        foreach (Vector2 move in legalMoves)
        {
            Square square = logicManager.GetSquareAtPosition(move);
            Piece pieceOnSquare = logicManager.boardMap[(int)move.x, (int)move.y];
            if (pieceOnSquare != null)
            {
                square.Highlight(Color.red);
            }
            else
            {
                square.Highlight(Color.cyan);
            }

            highlightedSquares.Add(square);
            //Debug.Log("Highlight square at: " + move);
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

    void MovePiece()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Square targetSquare = hit.transform.GetComponent<Square>();
            Piece targetPiece = hit.transform.GetComponent<Piece>();

            if (targetSquare != null && highlightedSquares.Contains(targetSquare))
            {
                Vector2 squareCoordinates = new Vector2(targetSquare.transform.position.x, targetSquare.transform.position.z);
                selectedPiece.Move(squareCoordinates);
                UnhighlightLegalMoves();
                selectedPiece = null;
                logicManager.UpdateCheckMap();
                logicManager.EndTurn();

            }
            else if (targetPiece != null)
            {
                Vector2 targetCoordinates = new Vector2(targetPiece.transform.position.x, targetPiece.transform.position.z);
                Square targetPieceSquare = logicManager.GetSquareAtPosition(targetCoordinates);

                if (highlightedSquares.Contains(targetPieceSquare))
                {
                    selectedPiece.Move(targetCoordinates);
                    UnhighlightLegalMoves();
                    selectedPiece = null;
                    logicManager.UpdateCheckMap();
                    logicManager.EndTurn();

                }
            }
        }
    }
}
