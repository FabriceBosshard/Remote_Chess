using System;
using System.Collections.ObjectModel;
using Chess.Pieces;

namespace Chess.Moves
{
    internal class RookMovement : Movement
    {
        private readonly Rook _rook;
        public ObservableCollection<ChessPieceViewModel> ActiveState;
        public RookMovement(Rook rook)
        {
            _rook = rook;
        }

       private bool IsPathToTargetFree(Field targetField)
        {
            return CanRookMoveLeft(targetField)
                   || CanRookMoveDown(targetField)
                   || CanRookMoveUp(targetField)
                   || CanRookMoveRight(targetField);
        }

        private bool CanRookMoveRight(Field targetField)
        {
            for (var i = 0; i < 8; i++)
            {
                //Moves to the right
                if (targetField.Column - _rook.Column == i && targetField.Row - _rook.Row == 0)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Column - _rook.Column) - 1;

                    //Check if piece blocks the path
                    foreach (var a in ActiveState)
                    {
                        for (var j = 1; j <= distance; j++)
                        {
                            if ((a.Row == _rook.Row) && (a.Column == targetField.Column - j))
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

        private bool CanRookMoveUp(Field targetField)
        {
            for (var i = 0; i < 8; i++)
            {
                //Moves to the right
                if (targetField.Row - _rook.Row == i && targetField.Column - _rook.Column == 0)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Row - _rook.Row) - 1;

                    //Check if piece blocks the path
                    foreach (var a in ActiveState)
                    {
                        for (var j = 1; j <= distance; j++)
                        {
                            if ((a.Column == _rook.Column) && (a.Row == targetField.Row - j))
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

        private bool CanRookMoveDown(Field targetField)
        {
            for (var i = 0; i < 8; i++)
            {
                //Moves to the right
                if (targetField.Row - _rook.Row == -i && targetField.Column - _rook.Column == 0)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Row - _rook.Row) - 1;

                    //Check if piece blocks the path
                    foreach (var a in ActiveState)
                    {
                        for (var j = 1; j <= distance; j++)
                        {
                            if ((a.Column == _rook.Column) && (a.Row == targetField.Row + j))
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

        private bool CanRookMoveLeft(Field targetField)
        {
            for (var i = 0; i < 8; i++)
            {
                //Moves to the right
                if (targetField.Column - _rook.Column == -i && targetField.Row - _rook.Row == 0)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Column - _rook.Column) - 1;

                    //Check if piece blocks the path
                    foreach (var a in ActiveState)
                    {
                        for (var j = 1; j <= distance; j++)
                        {
                            if ((a.Row == _rook.Row) && (a.Column == targetField.Column + j))
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
            ActiveState = activeState;
            return IsPathToTargetFree(targetField);
        }
    }
}