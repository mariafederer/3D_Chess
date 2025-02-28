using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public Piece[,] boardMap = new Piece[8,8];
    public Square[,] squares = new Square[8, 8];
    public bool[,] whiteCheckMap = new bool[8,8]; //potencjalne szachy dla czarnego krola
    public bool[,] blackCheckMap = new bool[8, 8]; //potencjlane szachy dla bialego krola
    public bool isWhiteTurn;
    public bool isCheck;
    public List<Piece> piecesOnBoard;

    public void Initialize()
    {
        isWhiteTurn = true;
        isCheck = false;
    }

    public void EndTurn()
    {
        isWhiteTurn = !isWhiteTurn;
        //Debug.Log(isWhiteTurn ? "White's turn" : "Black's turn");
    }
    public void UpdateCheckMap()
    {
        ResetCheckMap();
        UpdatePiecesOnBoard();

        foreach (Piece piece in piecesOnBoard)
        {
            if (piece != null)
            {
                List<Vector2> attackedFields = piece.GetAttackedFields();

                foreach (Vector2 field in attackedFields)
                {
                    if (piece.IsWhite)
                    {
                        whiteCheckMap[(int)field.x, (int)field.y] = true;
                    }
                    else
                    {
                        blackCheckMap[(int)field.x, (int)field.y] = true;
                    }
                }
            }
        }
    }

    private void ResetCheckMap()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                whiteCheckMap[x, y] = false;
                blackCheckMap[x, y] = false;
            }
        }
    }

    public Square GetSquareAtPosition(Vector2 position)
    {
        if (position.x < 0 || position.x >= squares.GetLength(0) || position.y < 0 || position.y >= squares.GetLength(1))
        {
            return null;
        }
        return squares[(int)position.x, (int)position.y];
    }
    public void CheckKingStatus()
    {
        King king = null;

        for (int x = 0; x < boardMap.GetLength(0); x++)
        {
            for (int y = 0; y < boardMap.GetLength(1); y++)
            {
                Piece piece = boardMap[x, y];
                if (piece is King && piece.IsWhite != isWhiteTurn)
                {
                    king = (King)piece;
                    break;
                }
            }
            if (king != null)
            {
                break;
            }
        }

        if (king != null)
        {
            Debug.Log("Checking if king is in check...");
            if (king.CheckForChecks())
            {
                Debug.Log("King is in check after the move!");
            }
            else
            {
                Debug.Log("King is not in check after the move.");
            }
        }
        else
        {
            Debug.LogError("King not found on the board.");
        }
    }

    private void UpdatePiecesOnBoard()
    {
        piecesOnBoard.Clear();
        for (int x = 0; x < boardMap.GetLength(0); x++)
        {
            for (int y = 0; y < boardMap.GetLength(1); y++)
            {
                Piece piece = boardMap[x, y];
                if (piece != null)
                {
                    piecesOnBoard.Add(piece);
                }
            }
        }
    }

}
