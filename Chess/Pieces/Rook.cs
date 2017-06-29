using Chess.Moves;
using Chess.Web_Services.PlayerService;

namespace Chess.Pieces
{
    public class Rook : ChessPieceViewModel
    {
        public override string ImageSource => "../ChessPiecesIMG/" + (IsBlack ? "Black" : "White") + "Rook" + ".png";

        protected override Movement GetMovementStrategy()
        {
            return new RookMovement(this);
        }

        public Rook(ChessPiece piece) : base(piece)
        {
            Type = ChessPieceEnum.Rook;
        }
        public Rook()
        {
            Type = ChessPieceEnum.Rook;
        }
    }
}
