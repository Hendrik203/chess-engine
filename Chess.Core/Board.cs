using System.Runtime.InteropServices;

namespace Chess.Core;

public class Board
{
    private Piece?[,] squares = new Piece?[8, 8];

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