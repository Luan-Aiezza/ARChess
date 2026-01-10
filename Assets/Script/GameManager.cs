using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject whitePiecePrefab;
    [SerializeField] private GameObject blackPiecePrefab;

    private Board board;
    private List<GameObject> spawnedPieces = new List<GameObject>();
    private Piece[,] boardState = new Piece[8, 8];

    public void OnBoardSpawned(GameObject boardObj)
    {
        board = boardObj.GetComponent<Board>();
        SpawnPieces();
    }

    private void SpawnPieces()
    {
        ClearPieces();
        
        // Peças brancas - linhas 0 a 2
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if ((x + y) % 2 == 1)
                    SpawnPiece(whitePiecePrefab, x, y);
            }
        }

        // Peças pretas - linhas 5 a 7
        for (int y = 5; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if ((x + y) % 2 == 1)
                    SpawnPiece(blackPiecePrefab, x, y);
            }
        }
    }

    private void SpawnPiece(GameObject prefab, int x, int y)
    {
        Vector3 pos = board.GetCellCenter(x, y);
        GameObject piece = Instantiate(prefab, pos, Quaternion.identity);
        var pieceComp = piece.GetComponent<Piece>();
        pieceComp.SetCoordinates(x, y);
        // set team based on prefab used
        if (prefab == whitePiecePrefab)
            pieceComp.team = Piece.Team.White;
        else
            pieceComp.team = Piece.Team.Black;

        boardState[x, y] = pieceComp;
        spawnedPieces.Add(piece);
    }

    public void ClearPieces()
    {
        foreach (var p in spawnedPieces)
            Destroy(p);

        spawnedPieces.Clear();
        boardState = new Piece[8, 8];
    }

    public Piece GetPieceAt(int x, int y)
    {
        if (x < 0 || x >= 8 || y < 0 || y >= 8) return null;
        return boardState[x, y];
    }

    public bool TryMovePiece(Piece piece, int tx, int ty)
    {
        if (piece == null || board == null) return false;

        int sx = piece.X;
        int sy = piece.Y;

        if (tx < 0 || tx >= board.gridSize || ty < 0 || ty >= board.gridSize) return false;

        // if same position, just snap back
        if (tx == sx && ty == sy)
        {
            piece.transform.position = board.GetCellCenter(sx, sy);
            return true;
        }

        // validate move
        int capturedX = -1, capturedY = -1;
        if (!IsValidMove(piece, tx, ty, out capturedX, out capturedY))
        {
            // invalid -> snap back
            piece.transform.position = board.GetCellCenter(sx, sy);
            return false;
        }

        // perform move
        boardState[sx, sy] = null;
        // if capture, remove captured piece
        if (capturedX != -1 && capturedY != -1)
        {
            var cap = boardState[capturedX, capturedY];
            if (cap != null)
            {
                spawnedPieces.Remove(cap.gameObject);
                Destroy(cap.gameObject);
                boardState[capturedX, capturedY] = null;
            }
        }

        // update piece coordinates and position
        piece.SetCoordinates(tx, ty);
        piece.transform.position = board.GetCellCenter(tx, ty);
        boardState[tx, ty] = piece;

        // crown if reached end
        if (!piece.isKing)
        {
            if (piece.team == Piece.Team.White && ty == board.gridSize - 1)
                piece.isKing = true;
            if (piece.team == Piece.Team.Black && ty == 0)
                piece.isKing = true;
        }

        return true;
    }

    // Validates simple diagonal move or single-jump capture
    private bool IsValidMove(Piece piece, int tx, int ty, out int capX, out int capY)
    {
        capX = -1; capY = -1;

        int sx = piece.X;
        int sy = piece.Y;

        if (GetPieceAt(tx, ty) != null) return false; // destination must be empty

        int dx = tx - sx;
        int dy = ty - sy;

        int adx = Mathf.Abs(dx);
        int ady = Mathf.Abs(dy);

        int dir = (piece.team == Piece.Team.White) ? 1 : -1;

        // simple move: one diagonal step forward (or backward if king)
        if (adx == 1 && ady == 1)
        {
            if (piece.isKing) return true;
            return (dy == dir);
        }

        // capture move: two diagonal steps, jumping over opponent
        if (adx == 2 && ady == 2)
        {
            int mx = sx + dx / 2;
            int my = sy + dy / 2;
            var mid = GetPieceAt(mx, my);
            if (mid == null) return false;
            if (mid.team == piece.team) return false;
            if (!piece.isKing && (dy != 2 * dir)) return false;
            capX = mx; capY = my;
            return true;
        }

        return false;
    }

    public void ResetGame()
    {
        SpawnPieces();
    }
}