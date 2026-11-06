using Avalonia.Controls; using Avalonia.Media; using Chess.Core;

namespace Chess.App;

public partial class MainWindow : Window
{
    private Board board = new Board();
    public MainWindow()
    {
        InitializeComponent();

        board.SetupStartingPosition();

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

                Piece? piece = board.GetPiece(row, col);

                if(piece != null)
                {
                    TextBlock text = new TextBlock();
                    text.Text = GetSymbol(piece);
                    text.FontSize = 48;
                    text.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                    text.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                    square.Child = text;
                    if (piece.Color == PieceColor.White)
                    {
                        text.Foreground = Brushes.White;
                    }
                    else
                    {
                        text.Foreground = Brushes.Black;
                    }
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