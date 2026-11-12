using System.Drawing;
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

    public List<(int row, int col)> GetPawnTargets(int row, int col)
    {
        List<(int row, int col)> targets = new List<(int row, int col)>();
        Piece? pawn = GetPiece(row, col);

        if (pawn == null)
        {
            return targets;
        }

        int direction;
        int startRow;

        if (pawn.Color == PieceColor.White)
        {
            direction = +1;
            startRow = 1;
        }
        else
        {
            direction = -1;
            startRow = 6;
        }

        int oneStep = row + direction;

        if (IsOnBoard(oneStep, col) && GetPiece(oneStep, col) == null)
        {
            targets.Add((oneStep, col));

            int twoSteps = row + 2 * direction;

            if (row == startRow && GetPiece(twoSteps, col) == null)
            {
                targets.Add((twoSteps, col));
            }
        }

        for (int side = -1; side <= 1; side += 2)
        {
            int captureCol = col + side;

            if (IsOnBoard(oneStep, captureCol))
            {
                Piece? other = GetPiece(oneStep, captureCol);

                if (other != null && other.Color != pawn.Color)
                {
                    targets.Add((oneStep, captureCol));
                }
            }
        }

        return targets;
    }

    public List<(int row, int col)> GetTargets(int row, int col)
    {
        Piece? piece = GetPiece(row, col);

        if (piece == null)
        {
            return new List<(int row, int col)>();
        }

        switch (piece.Type)
        {
            case PieceType.Knight:
                return GetKnightTargets(row, col);
            case PieceType.King:
                return GetKingTargets(row, col);
            case PieceType.Rook:
                return GetRookTargets(row, col);
            case PieceType.Bishop:
                return GetBishopTargets(row, col);
            case PieceType.Queen:
                return GetQueenTargets(row, col);
            case PieceType.Pawn:
                return GetPawnTargets(row, col);
            default:
                return new List<(int row, int col)>();
        }
    }

    public bool IsInCheck(PieceColor color)
    {
        int kingRow = -1;
        int kingCol = -1;

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece? piece = GetPiece(row, col);

                if (piece != null && piece.Type == PieceType.King && piece.Color == color)
                {
                    kingRow = row;
                    kingCol = col;
                }
            }
        }

        if (kingRow == -1)
        {
            return false;
        }

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece? piece = GetPiece(row, col);

                if (piece != null && piece.Color != color)
                {
                    if (GetTargets(row, col).Contains((kingRow, kingCol)))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
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
            List<(int row, int col)> KnightTargets = GetKnightTargets(fromRow, fromCol);
            return KnightTargets.Contains((toRow, toCol));
        }

        if (piece.Type == PieceType.King)
        {
            List<(int row, int col)> KingTargets = GetKingTargets(fromRow, fromCol);
            return KingTargets.Contains((toRow, toCol));
        }

        if (piece.Type == PieceType.Rook)
        {
            List<(int row, int col)> rookTargets = GetRookTargets(fromRow, fromCol);
            return rookTargets.Contains((toRow, toCol));
        }

        if (piece.Type == PieceType.Bishop)
        {
            List<(int row, int col)> bishopTargets = GetBishopTargets(fromRow, fromCol);
            return bishopTargets.Contains((toRow, toCol));
        }

        if (piece.Type == PieceType.Queen)
        {
            List<(int row, int col)> queenTargets = GetQueenTargets(fromRow, fromCol);
            return queenTargets.Contains((toRow, toCol));
        }

        if (piece.Type == PieceType.Pawn)
        {
            List<(int row, int col)> pawnTargets = GetPawnTargets(fromRow, fromCol);
            return pawnTargets.Contains((toRow, toCol));
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