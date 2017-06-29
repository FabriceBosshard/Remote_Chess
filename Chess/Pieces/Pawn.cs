using Chess.Moves;
using Chess.Web_Services.PlayerService;

namespace Chess.Pieces
{
    public class Pawn : ChessPieceViewModel
    {
        public override string ImageSource => "../ChessPiecesIMG/" + (IsBlack ? "Black" : "White") + "Pawn" + ".png";

        protected override Movement GetMovementStrategy()
        {
            return new PawnMovement(this);
        }

        public Pawn(ChessPiece piece) : base(piece)
        {
            
        }

        public Pawn()
        {
            Type = ChessPieceEnum.Pawn;
        }
    }
}
