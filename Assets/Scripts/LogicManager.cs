using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public Piece[,] boardMap = new Piece[8,8];
    public Square[,] squares = new Square[8, 8];
    public bool[,] whiteCheckMap = new bool[8,8];
    public bool[,] blackCheckMap = new bool[8, 8];
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
    }

    public void UpdateCheckMap()
    {
        ResetCheckMap();

        foreach (Piece piece in piecesOnBoard)
        {
            if (piece != null)
            {
                var attackedFields = piece.GetAttackedFields();

                foreach (var field in attackedFields)
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

}
