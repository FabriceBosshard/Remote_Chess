using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chess.Factory;
using Chess.Pieces;
using Chess.Web_Services.PlayerService;

namespace Chess.Moves
{
    public class MoveSimulator
    {        
        private readonly ChessPieceViewModel _kingUnderCheckTest;
        public readonly ObservableCollection<ChessPieceViewModel> _tempList = new ObservableCollection<ChessPieceViewModel>();
        public readonly ChessPieceViewModel _tempPiece;
        private readonly ChessPieceViewModel _piece;

        public MoveSimulator(ChessPieceViewModel piece, Field targetField)
        {
            _piece = piece;
            CopyGameState();
            _kingUnderCheckTest = GetKingUnderCheckTest(_piece.IsBlack);
            _tempPiece = GetTempPiece();    
            DoImaginaryMove(targetField);

        }

        public MoveSimulator(Player activePlayer)
        {
            CopyGameState();
            _kingUnderCheckTest = GetKingUnderCheckTest(activePlayer.IsBlack);
        }

        public MoveSimulator(King king)
        {
            CopyGameState();
            _kingUnderCheckTest = GetKingUnderCheckTest(king.IsBlack);
        }

        public bool CheckOnMultipleFields(List<Field> fieldsForCheckValidation)
        {
            foreach (var field in fieldsForCheckValidation)
            {
                if (IsKingInMovementPatternOfAny(field))
                {
                    return false;
                }
            }
            return true;
        }

        private void CopyGameState()
        {
            foreach (var chessPiece in Formation.Pieces)
            {
                _tempList.Add(new ChessPieceFactory().Create(chessPiece.ToDataContract()));
            }           
        }

        private void DoImaginaryMove(Field targetField)
        {
            foreach (var piece in _tempList)
            {
                if (piece.Row == _tempPiece.Row && piece.Column == _tempPiece.Column)
                {
                    foreach (var a in _tempList)
                    {
                        if (a.Row == targetField.Row && a.Column == targetField.Column)
                        {
                            if (a.IsBlack != piece.IsBlack)
                            {
                                _tempList.Remove(a);
                                piece.Row = targetField.Row;
                                piece.Column = targetField.Column;
                                break;
                            }
                        }
                    }
                    piece.Row = targetField.Row;
                    piece.Column = targetField.Column;

                    break;
                }
            }                      
        }

        private ChessPieceViewModel GetTempPiece()
        {
            return _tempList.Single(p => p.Row == _piece.Row && p.Column == _piece.Column);
        }

        private ChessPieceViewModel GetKingUnderCheckTest(bool isBlack)
        {
           return _tempList.Single(p => p.IsBlack == isBlack && p.Type == ChessPieceEnum.King);
        }

        public bool IsCheck()
        {
            Field targetField = new Field(_kingUnderCheckTest.Row, _kingUnderCheckTest.Column);
           

            return IsKingInMovementPatternOfAny(targetField);
        }

        private bool IsKingInMovementPatternOfAny(Field targetField)
        {
            foreach (var chessPiece in _tempList)
            {                
                if (chessPiece.IsBlack != _kingUnderCheckTest.IsBlack)
                {
                    if (!chessPiece.Type.Equals(ChessPieceEnum.King))
                    {                       
                        if (chessPiece.CanMove(targetField,_tempList))
                        {                                                             
                            return true;
                        } 
                    }
                    //If King -> uses other movementmethod to avoid recursion
                    else
                    {
                        King king = chessPiece as King;
                       
                        if (new KingMovement(king).IsPathToTargetFreeWhenCheckValidationOn(targetField,_tempList))
                        {
                            return true;
                        }
                    }                
                }
            }
            return false;
        }

        public bool IsCheckmate()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    foreach (var piece in _tempList)
                    {
                        if (_kingUnderCheckTest.IsBlack == piece.IsBlack)
                        {
                            Field field = new Field(i, j);

                            if (piece.TryMoveWithoutStateEffection(field,_tempList))
                            {                                                              
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}