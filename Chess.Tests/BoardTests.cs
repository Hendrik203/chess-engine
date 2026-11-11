using Chess.Core;

namespace Chess.Tests;

public class BoardTests
{
    [Fact]
    public void NewBoard_SquareIsEmpty()
    {
        Board board = new Board();

        Piece? piece = board.GetPiece(0, 0);

        Assert.Null(piece);
    }

    [Fact]
    public void SetPiece_ThenGetPiece_ReturnsSamePiece()
    {
        Board board = new Board();
        Piece queen = new Piece(PieceType.Queen, PieceColor.White);

        board.SetPiece(3, 4, queen);
        Piece? piece = board.GetPiece(3, 4);

        Assert.NotNull(piece);
        Assert.Equal(PieceType.Queen, piece.Type);
        Assert.Equal(PieceColor.White, piece.Color);
    }

    [Fact]
    public void SetupPawns_PlacesPawnsOnRowsTwoAndSeven()
    {
    
        Board board = new Board();

        board.SetupPawns();

        Piece? white = board.GetPiece(1, 4);   // e2
        Piece? black = board.GetPiece(6, 4);   // e7

        Assert.NotNull(white);
        Assert.NotNull(black);
        Assert.Equal(PieceType.Pawn, white.Type);
        Assert.Equal(PieceColor.White, white.Color);
        Assert.Equal(PieceType.Pawn, black.Type);
        Assert.Equal(PieceColor.Black, black.Color);

    }

    [Fact]
    public void SetupBackRanks_PlacesKingsAndRooks()
    {
        Board board = new Board();
        board.SetupBackRanks();

        Piece? white = board.GetPiece(0, 0); //a1
        Piece? black = board.GetPiece(7, 4); //e8

        Assert.NotNull(white);
        Assert.NotNull(black);
        Assert.Equal(PieceType.Rook, white.Type);
        Assert.Equal(PieceColor.White, white.Color);
        Assert.Equal(PieceType.King, black.Type);
        Assert.Equal(PieceColor.Black, black.Color);
    }

    [Fact]
    public void SetupStartingPosition_PlacesAllPieces()
    {
        Board board = new Board();
        board.SetupStartingPosition();

        Piece? e2 = board.GetPiece(1, 4); //e2
        Piece? e7 = board.GetPiece(6, 4); //e7
        Piece? d1 = board.GetPiece(0, 3); //d1
        Piece? e8 = board.GetPiece(7, 4); //e8
        Piece? e4 = board.GetPiece(3, 4); //e4

        Assert.Null(e4);
        Assert.NotNull(e2);
        Assert.NotNull(e7);
        Assert.NotNull(d1);
        Assert.NotNull(e8);
        Assert.Equal(PieceType.Pawn, e2.Type);
        Assert.Equal(PieceColor.White, e2.Color);
        Assert.Equal(PieceType.Pawn, e7.Type);
        Assert.Equal(PieceColor.Black, e7.Color);
        Assert.Equal(PieceType.Queen, d1.Type);
        Assert.Equal(PieceColor.White, d1.Color);
        Assert.Equal(PieceType.King, e8.Type);
        Assert.Equal(PieceColor.Black, e8.Color);
    }

    [Fact]
    public void MovePiece_MovesPawnFromE2ToE4()
    {
        Board board = new Board();
        board.SetupStartingPosition();

        board.MovePiece(1, 4, 3, 4);

        Piece? e4 = board.GetPiece(3, 4);
        Piece? e2 = board.GetPiece(1, 4);

        Assert.NotNull(e4);
        Assert.Equal(PieceType.Pawn, e4.Type);
        Assert.Equal(PieceColor.White, e4.Color);
        Assert.Null(e2);
    }

    [Fact]
    public void IsOnBoard_RecognizesFieldsInsideAndOutside()
    {
        Board board = new Board();

        Assert.True(board.IsOnBoard(0, 0));
        Assert.True(board.IsOnBoard(7, 7));
        Assert.False(board.IsOnBoard(-1, 3));
        Assert.False(board.IsOnBoard(3, 8));
    }

    [Fact]
    public void GetKnightTargets_CountsMovesCorrectly()
    {
        Board board = new Board();

        List<(int row, int col)> targets = board.GetKnightTargets(0, 1);

        Assert.Equal(3, targets.Count);
    }

    [Fact]
    public void IsLegalMove_KnightRules()
    {
        Board board = new Board();
        board.SetupStartingPosition();

        Assert.True(board.IsLegalMove(0, 1, 2, 2));
        Assert.False(board.IsLegalMove(0, 1, 2, 1));
        Assert.False(board.IsLegalMove(0, 1, 1, 3));
    }
}
