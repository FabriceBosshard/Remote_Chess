using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Chess.Pieces;

namespace Chess
{
    /// <summary>
    /// Interaktionslogik für PickPiecePopUp.xaml
    /// </summary>
    public partial class PickPiecePopUp
    {
        private readonly bool _black;
        public static ChessPieceViewModel SelectedPiece;
        public event EventHandler<EventArgs> TrapOccurred;

        protected virtual void OnTrapOccurred(EventArgs e)
        {
            TrapOccurred?.Invoke(this, e);
        }

        public PickPiecePopUp(bool black, ChessPieceViewModel piece)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
            _black = black;
            SelectedPiece = piece;
            CheckColor();
        }

        private void CheckColor()
        {          
            if (_black)
            {
                PickBishop.Background = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"../../ChessPiecesIMG/BlackBishop.png"), UriKind.Relative)));
                PickKnight.Background = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"../../ChessPiecesIMG/BlackKnight.png"), UriKind.Relative)));
                PickQueen.Background = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"../../ChessPiecesIMG/BlackQueen.png"), UriKind.Relative)));
                PickRook.Background = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"../../ChessPiecesIMG/BlackRook.png"), UriKind.Relative)));
            }
            else
            {
                PickBishop.Background = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"../../ChessPiecesIMG/WhiteBishop.png"),UriKind.Relative)));
                PickKnight.Background = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"../../ChessPiecesIMG/WhiteKnight.png"), UriKind.Relative)));
                PickQueen.Background = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"../../ChessPiecesIMG/WhiteQueen.png"), UriKind.Relative)));
                PickRook.Background = new ImageBrush(new BitmapImage(new Uri(Path.GetFullPath(@"../../ChessPiecesIMG/WhiteRook.png"), UriKind.Relative)));
            }
        }

        private void PickQueen_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedPiece = new Queen
            {
                Row = SelectedPiece.Row,
                Column = SelectedPiece.Column,
                IsBlack = SelectedPiece.IsBlack,
                IsSelected = SelectedPiece.IsSelected
            };
            PieceTransform();
        }

        private void PickRook_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedPiece = new Rook
            {
                Row = SelectedPiece.Row,
                Column = SelectedPiece.Column,
                IsBlack = SelectedPiece.IsBlack,
                IsSelected = SelectedPiece.IsSelected
            };
            PieceTransform();
        }

        private void PickBishop_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedPiece = new Bishop
            {
                Row = SelectedPiece.Row,
                Column = SelectedPiece.Column,
                IsBlack = SelectedPiece.IsBlack,
                IsSelected = SelectedPiece.IsSelected
            };
            PieceTransform();
        }

        private void PickKnight_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedPiece = new Knight
            {
                Row = SelectedPiece.Row,
                Column = SelectedPiece.Column,
                IsBlack = SelectedPiece.IsBlack,
                IsSelected = SelectedPiece.IsSelected
            };
            PieceTransform();
        }

        private void PieceTransform()
        {
            OnTrapOccurred(EventArgs.Empty);
            Close();
        }

    }
}
