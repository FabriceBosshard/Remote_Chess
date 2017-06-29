using System;
using System.Collections.ObjectModel;
using Chess.Pieces;

namespace Chess.Moves
{
    class BishopMovement : Movement
    {

        private readonly Bishop _bishop;
        private ObservableCollection<ChessPieceViewModel> _activeState;

        public BishopMovement(Bishop bishop)
        {
            _bishop = bishop;
        }

        private bool IsPathToTargetFree(Field targetField)
        {
            return CanBishopMoveUpLeft(targetField)
                   || CanBishopMoveDownLeft(targetField)
                   || CanBishopMoveUpRight(targetField)
                   || CanBishopMoveDownRight(targetField);
        }

        private bool CanBishopMoveDownRight(Field targetField)
        {
            for (int i = 1; i < 8; i++)
            {
                if (targetField.Row - _bishop.Row == i && targetField.Column - _bishop.Column == i)
                {
                   //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Column - _bishop.Column -1);

                    //Check if piece blocks the path
                    foreach (var a in _activeState)
                    {
                        for (var j = 1; j <= distance; j++)
                        {
                            if (a.Row == targetField.Row - j && a.Column == targetField.Column - j)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private bool CanBishopMoveUpRight(Field targetField)
        {
            for (int i = 1; i < 8; i++)
            {
                if (targetField.Row - _bishop.Row == -i && targetField.Column - _bishop.Column == i)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Column - _bishop.Column) - 1;

                    //Check if piece blocks the path
                    foreach (var a in _activeState)
                    {
                        for (var j = 1; j <= distance; j++)
                        {
                            if (a.Row == targetField.Row + j && a.Column == targetField.Column - j)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private bool CanBishopMoveDownLeft(Field targetField)
        {
            for (int i = 1; i < 8; i++)
            {
                if (targetField.Row - _bishop.Row == i && targetField.Column - _bishop.Column == -i)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Column - _bishop.Column) - 1;

                    //Check if piece blocks the path
                    foreach (var a in _activeState)
                    {
                        for (var j = 1; j <= distance; j++)
                        {
                            if (a.Row == targetField.Row - j && a.Column == targetField.Column + j)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private bool CanBishopMoveUpLeft(Field targetField)
        {
            for (int i = 1; i < 8; i++)
            {
                //moves diagonally?
                if (targetField.Row - _bishop.Row == -i && targetField.Column - _bishop.Column == -i)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Column - _bishop.Column) - 1;

                    //Check if piece blocks the path
                    foreach (var a in _activeState)
                    {
                        for (var j = 1; j <= distance; j++)
                        {
                            if (a.Row == targetField.Row + j && a.Column == targetField.Column + j)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public override bool CanMove(Field targetField, ObservableCollection<ChessPieceViewModel> activeState)
        {
            _activeState = activeState;
            return IsPathToTargetFree(targetField);
        }
    }
}

