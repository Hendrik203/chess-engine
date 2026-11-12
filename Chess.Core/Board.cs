using System.Runtime.InteropServices;

namespace Chess.Core;

public class Board
{
    private Piece?[,] squares = new Piece?[8, 8];

    private int[] knightRowOffsets = { 2, 2, -2, -2, 1, 1, -1, -1 };
    private int[] knightColOffsets = { 1, -1, 1, -1, 2, -2, 2, -2 };
    private int[] kingRowOffsets = { 1, 1, 1, 0, 0, -1, -1, -1 };
    private int[] kingColOffsets = { 1, 0, -1, 1, -1, 1, 0, -1 };
    private int[] rookRowDirs = { 1, -1, 0, 0 };
    private int[] rookColDirs = { 0, 0, 1, -1 };
    private int[] bishopRowDirs = { 1, 1, -1, -1 };
    private int[] bishopColDirs = { 1, -1, 1, -1 };
    private int[] QueenRowDirs = { 1, -1, 0, 0, 1, 1, -1, -1 };
    private int[] QueenColDirs = { 0, 0, 1, -1, 1, -1, 1, -1 };

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

    private List<(int row, int col)> GetStepTargets(int row, int col, int[] rowOffsets, int[] colOffsets)
    {
        List<(int row, int col)> targets = new List<(int row, int col)>();

        for (int i = 0; i < rowOffsets.Length; i++)
        {
            int newRow = row + rowOffsets[i];
            int newCol = col + colOffsets[i];

            if (IsOnBoard(newRow, newCol))
            {
                targets.Add((newRow, newCol));
            }
        }

        return targets;
    }

    private List<(int row, int col)> GetSlideTargets(int row, int col, int[] rowDirs, int[] colDirs)
    {
        List<(int row, int col)> targets = new List<(int row, int col)>();
        Piece? mover = GetPiece(row, col);

        for (int i = 0; i < rowDirs.Length; i++)
        {
            int newRow = row + rowDirs[i];
            int newCol = col + colDirs[i];

            while (IsOnBoard(newRow, newCol))
            {
                Piece? other = GetPiece(newRow, newCol);

                if (other == null)
                {
                    targets.Add((newRow, newCol));
                }
                else
                {
                    if (mover != null && other.Color != mover.Color)
                    {
                        targets.Add((newRow, newCol));
                    }
                    break;
                }

                newRow += rowDirs[i];
                newCol += colDirs[i];
            }
        }

        return targets;
    }

    public List<(int row, int col)> GetKnightTargets(int row, int col)
    {
        return GetStepTargets(row, col, knightRowOffsets, knightColOffsets);
    }

    public List<(int row, int col)> GetKingTargets(int row, int col)
    {
        return GetStepTargets(row, col, kingRowOffsets, kingColOffsets);
    }

    public List<(int row, int col)> GetRookTargets(int row, int col)
    {
        return GetSlideTargets(row, col, rookRowDirs, rookColDirs);
    }

    public List<(int row, int col)> GetBishopTargets(int row, int col)
    {
        return GetSlideTargets(row, col, bishopRowDirs, bishopColDirs);
    }

    public List<(int row, int col)> GetQueenTargets(int row, int col)
    {
        return GetSlideTargets(row, col, QueenRowDirs, QueenColDirs);
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

        if (piece.Type == PieceType.King)
        {
            List<(int row, int col)> targets = GetKingTargets(fromRow, fromCol);
            return targets.Contains((toRow, toCol));
        }

        if (piece.Type == PieceType.Bishop)
        {
            List<(int row, int col)> targets = GetBishopTargets(fromRow, fromCol);
            return targets.Contains((toRow, toCol));
        }

        if (piece.Type == PieceType.Queen)
        {
            List<(int row, int col)> targets = GetQueenTargets(fromRow, fromCol);
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