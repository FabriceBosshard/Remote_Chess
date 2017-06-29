using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chess.Pieces;

namespace Chess.Moves
{ 
    internal class KingMovement : Movement
    {
        private King _king;
        private ObservableCollection<ChessPieceViewModel> _activeState;

        public KingMovement(King king)
        {
            _king = king;
        }

        public KingMovement(ObservableCollection<ChessPieceViewModel> activeState, King king)
        {
            _activeState = activeState;
            _king = king;
        }

        private bool IsPathToTargetFree(Field targetField)
        {
            return CanMoveNormally(targetField)
                   || TryDoRochade(targetField);
        }

        public bool CanMoveNormally(Field targetField)
        {
            
            if (targetField.Row - _king.Row == -1 || targetField.Row - _king.Row == 1 ||
                targetField.Row - _king.Row == 0)
            {
                if (targetField.Column - _king.Column == 1 || targetField.Column - _king.Column == 0 ||
                    targetField.Column - _king.Column == -1)
                {
                    return true;
                }
            }
            return false;
        }

        private bool TryDoRochade(Field targetField)
        {
            if (TryIsRochadeAllowed(targetField))
            {                
                return TryPerformRochade(targetField);
            }
            return false;
        }

        private bool TryPerformRochade(Field targetField)
        {
            if (CheckAreSquaresToTargetNoChess(targetField))
            {
                return true;
            }
            return false;   
        }

        public void TryMoveLeftRookToRightOfKing(Field targetField)
        {
            var myRooks = _activeState.Where(p => p is Rook).Where(p => p.IsBlack == _king.IsBlack).ToList();
            if (myRooks.Any())
            {
                var rookToMove = myRooks.FirstOrDefault(r => r.Column < _king.Column);
                if (rookToMove != null)
                {
                    rookToMove.Column = _king.Column - 1;
                    rookToMove.FirstMove = false;
                }
            }
        }

        public void TryMoveRightRookToLeftOfKing(Field targetField)
        {
            var myRooks = _activeState.Where(p => p is Rook).Where(p => p.IsBlack == _king.IsBlack).ToList();
            if (myRooks.Any())
            {
                var rookToMove = myRooks.FirstOrDefault(r => r.Column > _king.Column);
                if (rookToMove != null)
                {
                    rookToMove.Column = _king.Column + 1;
                    rookToMove.FirstMove = false;
                }
            }
        }

        private bool TryIsRochadeAllowed(Field targetField)
        {
            return _king.FirstMove && TryRookFirstMove(targetField) && CheckAreSquaresToTargetFree(targetField) && IsFieldARochadeField(targetField);
        }

        private bool IsFieldARochadeField(Field targetField)
        {
            return targetField.Column == 0 || targetField.Column == 7;
        }

        private bool TryRookFirstMove(Field targetField)
        {
            var myRooks = _activeState.Where(p => p is Rook).Where(p => p.IsBlack == _king.IsBlack).ToList();
            if (TryMoveToLeft(targetField))
            {
                var leftRook = myRooks.FirstOrDefault(r => r.Column == 0);
                if (leftRook != null)
                {
                    return leftRook.FirstMove;
                }
                return false;
            }
            var rightRook = myRooks.FirstOrDefault(r => r.Column == 7);
            if (rightRook != null)
            {
                return rightRook.FirstMove;
            }
            return false;
        }

        private bool TryMoveToLeft(Field targetField)
        {
            return targetField.Column < _king.Column;
        }

        private bool CheckAreSquaresToTargetFree(Field targetField)
        {
            var currentColumn = _king.Column;
            var targetColumn = targetField.Column;
            var results = new List<bool>();

            if (targetColumn >= currentColumn)
            {
                for (int i = targetColumn; i > currentColumn; i--)
                {
                    results.Add(CheckIsFieldFree(i, _king.Row));
                }
            }
            else
            {
                for (int i = targetColumn; i < currentColumn; i++)
                {
                    results.Add(CheckIsFieldFree(i, _king.Row));
                }
            }
            return results.All(e => e);
        }

        private bool CheckIsFieldFree(int column, int row)
        {
            return !_activeState.Any(p => p.Column == column && p.Row == row);
        }


        private bool CheckAreSquaresToTargetNoChess(Field targetField)
        {
            var currentColumn = _king.Column;
            var targetColumn = targetField.Column;

            List<Field> fieldList = new List<Field>();

            if (targetColumn >= currentColumn)
            {
                for (int i = targetColumn; i > currentColumn; i--)
                {
                    fieldList.Add(new Field() { Column = i, Row = _king.Row });
                }
            }
            else
            {
                for (int i = targetColumn; i < currentColumn; i++)
                {
                    fieldList.Add(new Field() { Column = i, Row = _king.Row });
                }
            }

            return new MoveSimulator(_king).CheckOnMultipleFields(fieldList);               
        }

        public override bool CanMove(Field targetField, ObservableCollection<ChessPieceViewModel> activeState)
        {
            _activeState = activeState;
            return IsPathToTargetFree(targetField);
        }

        public bool IsPathToTargetFreeWhenCheckValidationOn(Field targetField,ObservableCollection<ChessPieceViewModel> activeState)
        {
            _activeState = activeState;
            return CanMoveNormally(targetField);
        }
    }
}
