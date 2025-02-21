using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    public Piece[,] boardMap = new Piece[8,8];
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


    /*
    public void Update()
    {
        DisplayBoard();
    }

    public List<Piece> piecesOnBoard;

    public void DisplayBoard()
    {
        string boardRepresentation = "";
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece piece = boardMap[row, col];

                if (piece != null)
                {
                    boardRepresentation += GetPieceSymbol(piece) + " ";
                }
                else
                {
                    boardRepresentation += ". ";
                }
            }
            boardRepresentation += "\n";
        }

        Debug.Log(boardRepresentation);
    }

    private string GetPieceSymbol(Piece piece)
    {
        if (piece.IsWhite)
        {
            switch (piece.PieceType)
            {
                case "Pawn": return "P";
                case "Rook": return "R";
                case "Knight": return "N";
                case "Bishop": return "B";
                case "Queen": return "Q";
                case "King": return "K";
                default: return "?"; // Dla nieznanego typu figury
            }
        }
        else
        {
            switch (piece.PieceType)
            {
                case "Pawn": return "p";
                case "Rook": return "r";
                case "Knight": return "n";
                case "Bishop": return "b";
                case "Queen": return "q";
                case "King": return "k";
                default: return "?"; // Dla nieznanego typu figury
            }
        }
    }
    */


}
