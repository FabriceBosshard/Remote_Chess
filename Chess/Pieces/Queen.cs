using Chess.Moves;
using Chess.Web_Services.PlayerService;

namespace Chess.Pieces
{
    public class Queen  : ChessPieceViewModel
    {
        public override string ImageSource => "../ChessPiecesIMG/" + (IsBlack ? "Black" : "White") + "Queen" + ".png";

        protected override Movement GetMovementStrategy()
        {
            return new QueenMovement(this);
        }

        public Queen(ChessPiece piece) : base(piece)
        {

        }

        public Queen()
        {
            Type = ChessPieceEnum.Queen;
        }
    }
}
