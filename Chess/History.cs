using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Factory;
using Chess.Pieces;
using Chess.Web_Services.PlayerService;

namespace Chess
{
    class History
    {
        public static Stack<ObservableCollection<ChessPieceViewModel>> GameStates = new Stack<ObservableCollection<ChessPieceViewModel>>();
        public static Stack<ObservableCollection<string>> FieldDiffs = new Stack<ObservableCollection<string>>();
        public static Stack<ObservableCollection<ChessPieceViewModel>> whiteDead = new Stack<ObservableCollection<ChessPieceViewModel>>();
        public static Stack<ObservableCollection<ChessPieceViewModel>> blackDead = new Stack<ObservableCollection<ChessPieceViewModel>>();

        public History(bool d)
        {
            GameStates.Push(Chessboard.oldState);
            FieldDiffs.Push(Chessboard.oldFieldDiff);
            blackDead.Push(Chessboard.oldBlackDeadPieces);
            whiteDead.Push(Chessboard.oldwhiteDeadPieces);

            if (GameStates.Count > 0)
            {
                Chessboard.Main.undo.IsEnabled = true;
            }
        }

        public History()
        {
            
        }
        public ObservableCollection<ChessPieceViewModel> cloneGameState()
        {
            var _tempList = new ObservableCollection<ChessPieceViewModel>();

            foreach (var chessPiece in Formation.Pieces)
            {
                _tempList.Add(new ChessPieceFactory().Create(chessPiece.ToDataContract()));
            }
            return _tempList;
        }

        public ObservableCollection<string> cloneDiffs()
        {
            var _tempList = new ObservableCollection<string>();

            foreach (var msg in Chessboard.stackMsg)
            {
                _tempList.Add(msg);
            }
            return _tempList;
        }

        public ObservableCollection<ChessPieceViewModel> cloneWhite()
        {
            var _tempList = new ObservableCollection<ChessPieceViewModel>();

            foreach (var chessPiece in Formation.WhiteDeadPieces)
            {
                _tempList.Add(new ChessPieceFactory().Create(chessPiece.ToDataContract()));
            }
            return _tempList;
        }

        public ObservableCollection<ChessPieceViewModel> cloneBlack()
        {
            var _tempList = new ObservableCollection<ChessPieceViewModel>();

            foreach (var chessPiece in Formation.BlackDeadPieces)
            {
                _tempList.Add(new ChessPieceFactory().Create(chessPiece.ToDataContract()));
            }
            return _tempList;
        }
    }
}
