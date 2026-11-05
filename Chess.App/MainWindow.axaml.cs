using Avalonia.Controls; using Avalonia.Media; using Chess.Core;

namespace Chess.App;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        for (int row = 7; row >= 0; row--)
        {
            for (int col = 0; col < 8; col++)
            {
                Border square = new Border();

                if ((row + col) % 2 == 0)
                {
                    square.Background = Brush.Parse("#703C29");             // dunkles Feld
                }
                else
                {
                    square.Background = Brush.Parse("#C1975E");             // helles Feld
                }

                BoardGrid.Children.Add(square);
            }
        }
    }

    private static string GetSymbol(Piece piece)
    {
        if (piece.Color == PieceColor.White)
        {
            switch (piece.Type)
            {
                case PieceType.King:
                    return "♔";
                case PieceType.Queen:
                    return "♕";
                case PieceType.Rook:
                    return "♖";
                case PieceType.Bishop:
                    return "♗";
                case PieceType.Knight:
                    return "♘";
                case PieceType.Pawn:
                    return "♙";
                default:
                    return "?";
            }
        }
        else
        {
            switch (piece.Type)
            {
                case PieceType.King:
                    return "♚";
                case PieceType.Queen:
                    return "♛";
                case PieceType.Rook:
                    return "♜";
                case PieceType.Bishop:
                    return "♝";
                case PieceType.Knight:
                    return "♞";
                case PieceType.Pawn:
                    return "♟";
                default:
                    return "?";
            }
        }
    }
}