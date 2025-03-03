using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    protected LogicManager logicManager;
    public bool IsWhite { get; private set; }
    public string PieceType { get; private set; }
    public int HasMoved { get; private set; }

    public abstract List<Vector2> GetAttackedFields();
    protected abstract List<Vector2> GetPotentialMoves();
    public virtual List<Vector2> GetLegalMoves()
    {
        List<Vector2> legalMoves = new List<Vector2>();
        bool isKingInCheck = logicManager.CheckKingStatus();
        foreach (Vector2 move in GetPotentialMoves())
        {
            if (WillMoveEndCheck(move))
            {
                legalMoves.Add(move);
            }
        }
        //Debug.Log($"Legal moves for {PieceType}: {legalMoves.Count}");

        return legalMoves;
    }

    protected bool WillMoveEndCheck(Vector2 move)
    {
        Piece originalPiece = logicManager.boardMap[(int)move.x, (int)move.y];
        Vector2 originalPosition = GetCoordinates();

        logicManager.boardMap[(int)originalPosition.x, (int)originalPosition.y] = null;
        logicManager.boardMap[(int)move.x, (int)move.y] = this;
        logicManager.UpdateCheckMap();

        bool isKingInCheck = logicManager.CheckKingStatus();

        if (this is King)
        {
            isKingInCheck = IsWhite ? logicManager.blackCheckMap[(int)move.x, (int)move.y] : logicManager.whiteCheckMap[(int)move.x, (int)move.y];
        }

        logicManager.boardMap[(int)originalPosition.x, (int)originalPosition.y] = this;
        logicManager.boardMap[(int)move.x, (int)move.y] = originalPiece;
        logicManager.UpdateCheckMap();

        return !isKingInCheck;
    }

    public void Initialize(string pieceType, bool isWhite)
    {
        PieceType = pieceType;
        IsWhite = isWhite;
        HasMoved = 0;
    }

    private void Start()
    {
        logicManager = Object.FindFirstObjectByType<LogicManager>();
        UpdateBoardMap();
    }

    public Vector2 GetCoordinates()
    {
        return new Vector2(transform.position.x, transform.position.z);
    }

    public void UpdateBoardMap()
    {
        Vector2 coordinates = GetCoordinates();
        logicManager.boardMap[(int)coordinates.x, (int)coordinates.y] = this;
    }

    public void Move(Vector2 newPosition)
    {
        Vector2 currentCoordinates = GetCoordinates();
        logicManager.boardMap[(int)currentCoordinates.x, (int)currentCoordinates.y] = null;
        HasMoved = 1;
        Take(newPosition);
        transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.y);
        UpdateBoardMap();
    }

    public void Take(Vector2 targetPosition)
    {
        Piece targetPiece = logicManager.boardMap[(int)targetPosition.x, (int)targetPosition.y];
        if (targetPiece != null)
        {
            Destroy(targetPiece.gameObject);
        }
        logicManager.boardMap[(int)targetPosition.x, (int)targetPosition.y] = null;
    }

    public bool IsPositionWithinBoard(Vector2 position)
    {
        return position.x >= 0 && position.x < 8 && position.y >= 0 && position.y < 8;
    }

}
