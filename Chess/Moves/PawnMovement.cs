using System.Collections.ObjectModel;
using System.Linq;
using Chess.Pieces;

namespace Chess.Moves
{
    class PawnMovement : Movement
    {
        private Pawn _pawn;
        private ObservableCollection<ChessPieceViewModel> activeState;

        public PawnMovement(Pawn pawn)
        {
            _pawn = pawn;
        }       

        private bool pawnAttackMoveWhite(Field targetField)
        {
            if ((targetField.Row - _pawn.Row == -1) && (targetField.Column - _pawn.Column == 1) ||
                (targetField.Row - _pawn.Row == -1) && (targetField.Column - _pawn.Column == -1))
            {
                return IsTargetFieldNotFree(targetField);
            }
            return false;
        }

        private bool pawnAttackMoveBlack(Field targetField)
        {
            if ((targetField.Row - _pawn.Row == 1) && (targetField.Column - _pawn.Column == 1) ||
                (targetField.Row - _pawn.Row == 1) && (targetField.Column - _pawn.Column == -1))
            {
                return IsTargetFieldNotFree(targetField);
            }
            return false;
        }

        private bool IsTargetFieldNotFree(Field targetField)
        {
            foreach (var b in activeState)
            {
                if ((b.Row == targetField.Row) && (b.Column == targetField.Column))
                {
                    return true;
                }
            }
            return false;
        }       

        private bool pawnDoubleMoveWhite(Field targetField)
        {
            if (_pawn.FirstMove)
            {
                if ((targetField.Row - _pawn.Row == -2) && (targetField.Column - _pawn.Column == 0) && IsTargetFieldFree(targetField))
                {
                    _pawn.IsEnPassantEnabled = true;
                    return true;
                }
            }
            return false;
        }

        private bool IsTargetFieldFree(Field targetField)
        {
            foreach (var b in activeState)
            {
                if ((b.Row == targetField.Row) && (b.Column == targetField.Column))
                {
                    return false;
                }
            }
            return true;
        }

        private bool pawnDoubleMoveBlack(Field targetField)
        {
            if (_pawn.FirstMove)
            {
                if ((targetField.Row - _pawn.Row == 2) && (targetField.Column - _pawn.Column == 0) && IsTargetFieldFree(targetField))
                {
                    _pawn.IsEnPassantEnabled = true;
                    return true;
                }
            }
            return false;
        }

        private bool pawnEnPassantWhite(Field targetField)
        {
            if (((targetField.Row - _pawn.Row == -1) && (targetField.Column - _pawn.Column == 1)) ||
                 ((targetField.Row - _pawn.Row == -1) && (targetField.Column - _pawn.Column == -1)))
            {
                var pawnToEat = activeState.OfType<Pawn>()
                    .SingleOrDefault(pawn =>
                        pawn.IsBlack != _pawn.IsBlack &&
                        pawn.IsEnPassantEnabled &&
                        pawn.Column == targetField.Column &&
                        pawn.Row - 1 == targetField.Row);
                if (pawnToEat != null)
                {
                    Formation.Pieces.Remove(pawnToEat);
                    if (!pawnToEat.IsBlack)
                    {
                        Formation.WhiteDeadPieces.Add(pawnToEat);

                    }
                    else{
                        Formation.BlackDeadPieces.Add(pawnToEat);

                    }
                    return true;
                }
            }
            return false;
        }

        private bool pawnNormalMoveWhite(Field targetField)
        {
            if ((targetField.Row - _pawn.Row == -1) && (targetField.Column - _pawn.Column == 0) && IsTargetFieldFree(targetField))
            {
                return true;
            }
            return false;
        }

        private bool pawnNormalMoveBlack(Field targetField)
        {
            if ((targetField.Row - _pawn.Row == 1) && (targetField.Column - _pawn.Column == 0) && IsTargetFieldFree(targetField))
            {
                return true;
            }
            return false;
        }

        private bool pawnEnPassantBlack(Field targetField)
        {
            if (((targetField.Row - _pawn.Row == 1) && (targetField.Column - _pawn.Column == 1)) ||
                ((targetField.Row - _pawn.Row == 1) && (targetField.Column - _pawn.Column == -1)))
            {
                var pawnToEat = activeState.OfType<Pawn>()
                    .SingleOrDefault(pawn =>
                        pawn.IsBlack != _pawn.IsBlack &&
                        pawn.IsEnPassantEnabled &&
                        pawn.Column == targetField.Column &&
                        pawn.Row + 1 == targetField.Row);
                if(pawnToEat != null)
                {    
                    Formation.Pieces.Remove(pawnToEat);
                    if (!pawnToEat.IsBlack)
                    {
                        Formation.WhiteDeadPieces.Add(pawnToEat);

                    }
                    else{
                        Formation.BlackDeadPieces.Add(pawnToEat);

                    }
                    return true;                       
                }
            }
            return false;
        }


        private bool IsMoveForBlackAllowed(Field targetField)
        {
            return pawnAttackMoveBlack( targetField)
                   || pawnDoubleMoveBlack( targetField)
                   || pawnNormalMoveBlack(targetField)
                   || pawnEnPassantBlack(targetField);
        }

        private bool IsMoveForWhiteAllowed(Field targetField)
        {
            return pawnAttackMoveWhite(targetField)
                   || pawnDoubleMoveWhite(targetField)
                   || pawnNormalMoveWhite(targetField)
                   || pawnEnPassantWhite(targetField);
        }

        public override bool CanMove(Field targetField, ObservableCollection<ChessPieceViewModel> activeState)
        {
            this.activeState = activeState;
            if (_pawn.IsBlack)
            {
                return IsMoveForBlackAllowed(targetField);
            }
            return IsMoveForWhiteAllowed(targetField);
        }
    }
}
