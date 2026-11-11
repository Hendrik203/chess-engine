using System.Runtime.InteropServices;

namespace Chess.Core;

public class Board
{
    private Piece?[,] squares = new Piece?[8, 8];

    private int[] knightRowOffsets = { 2, 2, -2, -2, 1, 1, -1, -1 };
    private int[] knightColOffsets = { 1, -1, 1, -1, 2, -2, 2, -2 };

    public Piece? GetPiece(int row, int col)
    {
        return squares[row, col];
    }

    public void SetPiece(int row, int col, Piece? piece)
    {
        squares[row, col] = piece;
    }

    public void MovePiece(int fromRow, int fromCol, int toRow, int toCol)
    {
        if (fromRow == toRow && fromCol == toCol)
        {
            return;
        }

        Piece? piece = GetPiece(fromRow,fromCol);
        SetPiece(toRow, toCol, piece);
        SetPiece(fromRow, fromCol, null);
    }

    public bool IsOnBoard(int row, int col)
    {
        return row >= 0 && row < 8 && col >= 0 && col < 8;
    }

    public List<(int row, int col)> GetKnightTargets(int row, int col)
    {
        List<(int row, int col)> targets = new List<(int row, int col)>();

        for (int i = 0; i < 8; i++)
        {
            int newRow = row + knightRowOffsets[i];
            int newCol = col + knightColOffsets[i];

            if (IsOnBoard(newRow, newCol))
            {
                targets.Add((newRow, newCol));
            }
        }

        return targets;
    }

        public bool IsLegalMove(int fromRow, int fromCol, int toRow, int toCol)
    {
        Piece? piece = GetPiece(fromRow, fromCol);
        if (piece == null)
        {
            return false;
        }

        Piece? target = GetPiece(toRow, toCol);
        if (target != null && target.Color == piece.Color)
        {
            return false;
        }

        if (piece.Type == PieceType.Knight)
        {
            List<(int row, int col)> targets = GetKnightTargets(fromRow, fromCol);
            return targets.Contains((toRow, toCol));
        }

        return true;
    }
    
    public void SetupPawns()
    {
        for(int col = 0; col < 8; col++)
        {
            SetPiece(1, col, new Piece(PieceType.Pawn, PieceColor.White));
            SetPiece(6, col, new Piece(PieceType.Pawn, PieceColor.Black));
        }
    }

    public void SetupBackRanks()
    {
        PieceType[] backRank =
        {
            PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen,
            PieceType.King, PieceType.Bishop, PieceType.Knight, PieceType.Rook
        };

        for (int col = 0; col < 8; col++)
        {
            SetPiece(0, col, new Piece(backRank[col], PieceColor.White));
            SetPiece(7, col, new Piece(backRank[col], PieceColor.Black));
        }
    }

    public void SetupStartingPosition()
    {
        SetupPawns();
        SetupBackRanks();
    }
}