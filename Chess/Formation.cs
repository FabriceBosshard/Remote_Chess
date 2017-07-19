using System.Collections.ObjectModel;
using System.Linq;
using Chess.Pieces;

namespace Chess
{
    public class Formation
    {
        public static ObservableCollection<ChessPieceViewModel> Pieces = new ObservableCollection<ChessPieceViewModel>();
        public static ObservableCollection<ChessPieceViewModel> BlackDeadPieces = new ObservableCollection<ChessPieceViewModel>();
        public static ObservableCollection<ChessPieceViewModel> WhiteDeadPieces = new ObservableCollection<ChessPieceViewModel>();

        private static void NewFormation()
        {              
            Pieces.Clear();
            BlackDeadPieces.Clear();
            WhiteDeadPieces.Clear();
            Chessboard.stackMsg.Clear();
            Chessboard.oldBlackDeadPieces.Clear();
            Chessboard.oldFieldDiff.Clear();
            Chessboard.oldwhiteDeadPieces.Clear();
            Chessboard.oldState.Clear();
            History.GameStates.Clear();
            History.FieldDiffs.Clear();
            History.blackDead.Clear();
            History.whiteDead.Clear();

            Pieces.Add(new Rook() { Row = 0, Column = 0, IsBlack = true });
            Pieces.Add(new Knight() { Row = 0, Column = 1, IsBlack = true });
            Pieces.Add(new Bishop() { Row = 0, Column = 2, IsBlack = true });
            Pieces.Add(new Queen() { Row = 0, Column = 3, IsBlack = true });
            Pieces.Add(new King() { Row = 0, Column = 4, IsBlack = true });
            Pieces.Add(new Bishop() { Row = 0, Column = 5, IsBlack = true });
            Pieces.Add(new Knight() { Row = 0, Column = 6, IsBlack = true });
            Pieces.Add(new Rook() { Row = 0, Column = 7, IsBlack = true });

            Enumerable.Range(0, 8).Select(x => new Pawn()
            {
                Row = 1,
                Column = x,
                IsBlack = true,              
            }).ToList().ForEach(Pieces.Add);


            Pieces.Add(new Rook() { Row = 7, Column = 0, IsBlack = false });
            Pieces.Add(new Knight() { Row = 7, Column = 1, IsBlack = false });
            Pieces.Add(new Bishop() { Row = 7, Column = 2, IsBlack = false });
            Pieces.Add(new Queen() { Row = 7, Column = 3, IsBlack = false });
            Pieces.Add(new King() { Row = 7, Column = 4, IsBlack = false });
            Pieces.Add(new Bishop() { Row = 7, Column = 5, IsBlack = false });
            Pieces.Add(new Knight() { Row = 7, Column = 6, IsBlack = false });
            Pieces.Add(new Rook() { Row = 7, Column = 7, IsBlack = false });

            Enumerable.Range(0, 8).Select(x => new Pawn()
            {
                Row = 6,
                Column = x,
                IsBlack = false,
            }).ToList().ForEach(Pieces.Add);          
        }

        public void Initialize()
        {
            NewFormation();
        }
    }
}
