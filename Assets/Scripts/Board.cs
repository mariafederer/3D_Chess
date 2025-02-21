using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject Square;
    public GameObject[] PiecePrefabs;
    public Material[] PieceMaterials;
    public int Width = 8;
    public int Height = 8;

    void Start()
    {
        GenerateBoard();
        PlaceStartingPosition();
    }

    public void GenerateBoard()
    {
        for (int i = 0; i < Width; i++) {
            for (int j = 0; j < Height; j++) {
                GameObject square = Instantiate(Square, new Vector3(i, 0, j), Quaternion.identity);
                square.transform.parent = this.transform;
                Renderer renderer = square.GetComponent<Renderer>();
                if ((i + j) % 2 == 0)
                {
                    renderer.material.color = Color.black;
                }
                else
                {
                    renderer.material.color = Color.white;
                }
            }
        }
    }

    public void PlaceStartingPosition() {
        float pieceHeight = 0.12f;
        float pawnsHeight = 0.05f;

        for (int i = 0; i < Width; i++)
        {
            InstantiatePiece(PiecePrefabs[0], new Vector3(i, pawnsHeight, 1), PieceMaterials[0], "Pawn", true);
            InstantiatePiece(PiecePrefabs[1], new Vector3(i, pawnsHeight, 6), PieceMaterials[1], "Pawn", false);
        }

        InstantiatePiece(PiecePrefabs[2], new Vector3(0, pieceHeight, 0), PieceMaterials[0], "Rook", true);
        InstantiatePiece(PiecePrefabs[2], new Vector3(7, pieceHeight, 0), PieceMaterials[0], "Rook", true);
        InstantiatePiece(PiecePrefabs[3], new Vector3(0, pieceHeight, 7), PieceMaterials[1], "Rook", false);
        InstantiatePiece(PiecePrefabs[3], new Vector3(7, pieceHeight, 7), PieceMaterials[1], "Rook", false);

        InstantiatePiece(PiecePrefabs[4], new Vector3(1, pieceHeight, 0), PieceMaterials[0], "Knight", true);
        InstantiatePiece(PiecePrefabs[4], new Vector3(6, pieceHeight, 0), PieceMaterials[0], "Knight", true);
        InstantiatePiece(PiecePrefabs[5], new Vector3(1, pieceHeight, 7), PieceMaterials[1], "Knight", false);
        InstantiatePiece(PiecePrefabs[5], new Vector3(6, pieceHeight, 7), PieceMaterials[1], "Knight", false);

        InstantiatePiece(PiecePrefabs[6], new Vector3(2, pieceHeight, 0), PieceMaterials[0], "Bishop", true);
        InstantiatePiece(PiecePrefabs[6], new Vector3(5, pieceHeight, 0), PieceMaterials[0], "Bishop", true);
        InstantiatePiece(PiecePrefabs[7], new Vector3(2, pieceHeight, 7), PieceMaterials[1], "Bishop", false);
        InstantiatePiece(PiecePrefabs[7], new Vector3(5, pieceHeight, 7), PieceMaterials[1], "Bishop", false);

        InstantiatePiece(PiecePrefabs[8], new Vector3(3, pieceHeight, 0), PieceMaterials[0], "Queen", true);
        InstantiatePiece(PiecePrefabs[9], new Vector3(3, pieceHeight, 7), PieceMaterials[1], "Queen", false);

        InstantiatePiece(PiecePrefabs[10], new Vector3(4, pieceHeight, 0), PieceMaterials[0], "King", true);
        InstantiatePiece(PiecePrefabs[11], new Vector3(4, pieceHeight, 7), PieceMaterials[1], "King", false);
    }

    private void InstantiatePiece(GameObject piecePrefab, Vector3 position, Material material, string pieceType, bool isWhite)
    {
        GameObject pieceObject = Instantiate(piecePrefab, position, Quaternion.identity);
        pieceObject.transform.parent = this.transform;
        Renderer renderer = pieceObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
        Piece piece = pieceObject.GetComponent<Piece>();
        if (piece != null)
        {
            piece.Initialize(pieceType, isWhite);
            //Debug.Log($"Instantiated {pieceType} at {position} with IsWhite={isWhite}");
        }
    }
}
