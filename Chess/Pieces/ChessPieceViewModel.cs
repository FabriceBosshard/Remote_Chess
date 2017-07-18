using System;
using System.Collections.ObjectModel;
using Chess.Interfaces;
using Chess.Moves;
using Chess.Web_Services.PlayerService;

namespace Chess.Pieces
{
    public abstract class ChessPieceViewModel : NotifyPropertyChangedViewModel
    {

        public abstract string ImageSource { get; }

        public ChessPieceEnum Type { get; set; }
        

        protected string Id { get; private set; }

        protected ChessPieceViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }


       public void UpdateChessPieceViewModel(ChessPiece piece)
        {
                FirstMove = piece.FirstMove;
                IsEnPassantEnabled = piece.IsEnPassantEnabled;
                Row = piece.Row;
                Column = piece.Column;
                Id = piece.Id;
                IsBlack = piece.IsBlack;
                ChessPieceEnum res;
                Enum.TryParse(piece.Type, out res);
                Type = res;
            
        }

        protected ChessPieceViewModel(ChessPiece piece)
        {
            UpdateChessPieceViewModel(piece);
        }


        public bool IsBlack { get; set; }

        public bool FirstMove = true;

        public bool IsEnPassantEnabled;

        public bool IsSelected { get; set; }

        private int _row;

        public int Row
        {
            get { return _row; }
            set
            {
                _row = value;
                OnPropertyChanged();
            }
        }

        private int _column;

        public int Column
        {
            get { return _column; }
            set
            {
                _column = value;
                OnPropertyChanged();
            }
        }

        public ChessPiece ToDataContract()
        {
            return new ChessPiece
            {
                Id = Id,
                Row = Row,
                Column = Column,               
                FirstMove = FirstMove,
                IsBlack = IsBlack,
                IsEnPassantEnabled = IsEnPassantEnabled,
                IsSelected = IsSelected,               
                Type = Type.ToString()
            };
        }

        public void TryMove(Field targetField, ObservableCollection<ChessPieceViewModel> activeState)
        {
            if (CanMove(targetField,activeState) && !NoChess(targetField))
            {
                if (IsTargetFieldFree(targetField,activeState))
                {
                     Move(targetField);
                }
                else
                {
                    TryConsumeAtTargetField(targetField);
                }               
            }
            else
            {
                Chessboard.Main.MSGLabel.Content = "Your move is not allowed (Tip: Wrong movementpattern | Is check after move)";
            }
        }

        public bool NoChess(Field targetField)
        {
            if (new MoveSimulator(this, targetField).IsCheck())
            {                
                return true;
            }
            return false;
        }

        private bool IsTargetFieldFree(Field target, ObservableCollection<ChessPieceViewModel> activeState)
        {
            foreach (var b in activeState)
            {
                if ((b.Row == target.Row) && (b.Column == target.Column))
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanMove(Field field, ObservableCollection<ChessPieceViewModel>activeState )
        {
            return GetMovementStrategy().CanMove(field, activeState);
        }
        public bool CanMove(Field field)
        {
            return GetMovementStrategy().CanMove(field);
        }

        protected abstract Movement GetMovementStrategy();

        protected virtual void Move(Field target)
        {
            if (Type != ChessPieceEnum.King)
            {
                new PushToStack(target, this.Column, this.Row, this);
            }
            Column = target.Column;
            Row = target.Row;

            if (Chessboard.oldState!=null)
            {
                new History(true);
            }
            Chessboard.oldState = new History().cloneGameState();
            Chessboard.oldFieldDiff = new History().cloneDiffs();
            Chessboard.oldwhiteDeadPieces = new History().cloneWhite();
            Chessboard.oldBlackDeadPieces = new History().cloneBlack();


            ChessPieceMove.SwapPlayer = true;
            if (!FirstMove && ChessPieceMove.SwapPlayer)
            {
                foreach (var a in Formation.Pieces)
                {
                    a.IsEnPassantEnabled = false;
                }
            }
            FirstMove = false;
            
        }

        public void TryConsumeAtTargetField(Field target)
        {
            foreach (var b in Formation.Pieces)
            {
                //Check if targetField field has piece
                if ((b.Row == target.Row) && (b.Column == target.Column))
                    //Check if it is same color
                    if (b.IsBlack != IsBlack)
                    {
                        if (IsBlack)
                        {
                            Formation.WhiteDeadPieces.Add(b);

                        }
                        else{
                            Formation.BlackDeadPieces.Add(b);
                        }
                        Formation.Pieces.Remove(b);
                        Move(target);
                        break;
                    }
            }
        }

        public bool TryMoveWithoutStateEffection(Field field, ObservableCollection<ChessPieceViewModel> activeState)
        {
            if (CanMove(field, activeState) && !NoChess(field))
            {
                if (IsTargetFieldFree(field, activeState))
                {
                    return MoveWithoutStateEffection(field, activeState);
                }
                return TryConsumeAtTargetFieldWithoutStateEffection(field,activeState);
            }
            return false;
        }

        private bool TryConsumeAtTargetFieldWithoutStateEffection(Field field, ObservableCollection<ChessPieceViewModel> activeState)
        {
            foreach (var b in activeState)
            {
                //Check if targetField field has piece
                if ((b.Row == field.Row) && (b.Column == field.Column))
                    //Check if it is same color
                    if (b.IsBlack != IsBlack)
                    {
                        MoveWithoutStateEffection(field,activeState);
                        return true;
                    }
            }
            return false;
        }

        private bool MoveWithoutStateEffection(Field field, ObservableCollection<ChessPieceViewModel> activeState)
        {
            Column = field.Column;
            Row = field.Row;

            if (!FirstMove && ChessPieceMove.SwapPlayer)
            {
                foreach (var a in activeState)
                {
                    a.IsEnPassantEnabled = false;
                }
            }
            FirstMove = false;

            return true;
        }
    }
}
