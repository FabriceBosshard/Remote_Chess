using System;
using System.Collections.ObjectModel;
using Chess.Pieces;

namespace Chess.Moves
{
    class QueenMovement : Movement
    {
        private readonly Queen _queen;
        private ObservableCollection<ChessPieceViewModel> _activeState;

        public QueenMovement(Queen queen)
        {
            _queen = queen;
        }

        private bool IsPathToTargetFree(Field targetField)
        {
            return CanQueenMoveLeft(targetField)
                   || CanQueenMoveDown(targetField)
                   || CanQueenMoveUp(targetField)
                   || CanQueenMoveRight(targetField)
                   || CanQueenMoveDownRight(targetField)
                   || CanQueenMoveUpRight(targetField)
                   || CanQueenMoveDownLeft(targetField)
                   || CanQueenMoveUpLeft(targetField);
        }

        private bool CanQueenMoveUpLeft(Field targetField)
        {
            for (int i = 1; i < 8; i++)
            {
                //moves diagonally?
                if (targetField.Row - _queen.Row == -i && targetField.Column - _queen.Column == -i)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Column - _queen.Column) - 1;

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

        private bool CanQueenMoveUpRight(Field targetField)
        {
            for (int i = 1; i < 8; i++)
            {
                if (targetField.Row - _queen.Row == -i && targetField.Column - _queen.Column == i)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Column - _queen.Column) - 1;

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

        private bool CanQueenMoveDownLeft(Field targetField)
        {
            for (int i = 1; i < 8; i++)
            {
                if (targetField.Row - _queen.Row == i && targetField.Column - _queen.Column == -i)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Column - _queen.Column) - 1;

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

        private bool CanQueenMoveDownRight(Field targetField)
        {
            for (int i = 1; i < 8; i++)
            {
                if (targetField.Row - _queen.Row == i && targetField.Column - _queen.Column == i)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Column - _queen.Column - 1);

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

        private bool CanQueenMoveRight(Field targetField)
        {
            for (var i = 0; i < 8; i++)
            {
                //Moves to the right
                if (targetField.Column - _queen.Column == i && targetField.Row - _queen.Row == 0)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Column - _queen.Column) - 1;

                    //Check if piece blocks the path
                    foreach (var a in _activeState)
                    {
                        for (var j = 1; j <= distance; j++)
                        {
                            if ((a.Row == _queen.Row) && (a.Column == targetField.Column - j))
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

        private bool CanQueenMoveUp(Field targetField)
        {
            for (var i = 0; i < 8; i++)
            {
                //Moves to the right
                if (targetField.Row - _queen.Row == i && targetField.Column - _queen.Column == 0)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Row - _queen.Row) - 1;

                    //Check if piece blocks the path
                    foreach (var a in _activeState)
                    {
                        for (var j = 1; j <= distance; j++)
                        {
                            if ((a.Column == _queen.Column) && (a.Row == targetField.Row - j))
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

        private bool CanQueenMoveDown(Field targetField)
        {
            for (var i = 0; i < 8; i++)
            {
                //Moves to the right
                if (targetField.Row - _queen.Row == -i && targetField.Column - _queen.Column == 0)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Row - _queen.Row) - 1;

                    //Check if piece blocks the path
                    foreach (var a in _activeState)
                    {
                        for (var j = 1; j <= distance; j++)
                        {
                            if ((a.Column == _queen.Column) && (a.Row == targetField.Row + j))
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

        private bool CanQueenMoveLeft(Field targetField)
        {
            for (var i = 0; i < 8; i++)
            {
                //Moves to the right
                if (targetField.Column - _queen.Column == -i && targetField.Row - _queen.Row == 0)
                {
                    //Get distance of targetfield and position
                    var distance = Math.Abs(targetField.Column - _queen.Column) - 1;

                    //Check if piece blocks the path
                    foreach (var a in _activeState)
                    {
                        for (var j = 1; j <= distance; j++)
                        {
                            if ((a.Row == _queen.Row) && (a.Column == targetField.Column + j))
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


        public override bool CanMove(Field targetField)
        {
           return CanMove(targetField, Formation.Pieces);
        }

        public override bool CanMove(Field targetField, ObservableCollection<ChessPieceViewModel> activeState)
        {
            _activeState = activeState;
            return IsPathToTargetFree(targetField);
        }
    }
}
