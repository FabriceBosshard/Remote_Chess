using Chess.Interfaces;
using Chess.Pieces;

namespace Chess.Moves
{
    public class ChessPieceMove : NotifyPropertyChangedViewModel
    {
        public static bool SwapPlayer = false;

        public void ValidateMove(ChessPieceViewModel piece, Field targetField)
        {   
            piece.TryMove(targetField,Formation.Pieces); 
        }
    }
}