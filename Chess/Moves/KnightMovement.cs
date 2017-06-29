using System.Collections.ObjectModel;
using Chess.Pieces;

namespace Chess.Moves
{
    class KnightMovement : Movement
    {

        private Knight _knight;
        public KnightMovement(Knight knight)
        {
            _knight = knight;
        }

        private bool IsPathToTargetFree(Field targetField)
        {
            return TryCheckForFieldsUpAndDown(targetField)
                   || TryCheckForFieldsRightAndLeft(targetField);
        }

        private bool TryCheckForFieldsUpAndDown(Field targetField)
        {
            if (targetField.Column - _knight.Column == 2 || targetField.Column - _knight.Column == -2)
            {
                if (targetField.Row - _knight.Row == -1 || targetField.Row - _knight.Row == 1)
                {
                    return true;
                }
            }
            return false;
        }

        private bool TryCheckForFieldsRightAndLeft(Field targetField)
        {
            if (targetField.Row - _knight.Row == 2 || targetField.Row - _knight.Row == -2)
            {
                if (targetField.Column - _knight.Column == -1 || targetField.Column - _knight.Column == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CanMove(Field targetField, ObservableCollection<ChessPieceViewModel> activeState)
        {
            return IsPathToTargetFree(targetField);
        }
    }
}
