using Chess.Moves;
using Chess.Web_Services.PlayerService;

namespace Chess.Pieces
{
    public class Bishop : ChessPieceViewModel
    {
        public override string ImageSource => "../ChessPiecesIMG/" + (IsBlack ? "Black" : "White") + "Bishop" + ".png";

        protected override Movement GetMovementStrategy()
        {
            return new BishopMovement(this);
        }

        public Bishop(ChessPiece piece) : base(piece)
        {
            
        }

        //public new bool CanMove(Field targetField)
        //{
        //  return base.CanMove(targetField);
        //}

        public Bishop()
        {
            Type = ChessPieceEnum.Bishop;
        }
    }
}
