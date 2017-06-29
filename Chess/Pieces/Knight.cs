using Chess.Moves;
using Chess.Web_Services.PlayerService;

namespace Chess.Pieces
{
    public class Knight : ChessPieceViewModel
    {
        public override string ImageSource => "../ChessPiecesIMG/" + (IsBlack ? "Black" : "White") + "Knight" + ".png";
        protected override Movement GetMovementStrategy()
        {
            return new KnightMovement(this);
        }

        public Knight(ChessPiece piece) : base(piece)
        {
            Type = ChessPieceEnum.Knight;
        }

        public Knight()
        {
            Type = ChessPieceEnum.Knight;
        }
    }
}
