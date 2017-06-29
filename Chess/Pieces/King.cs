using System.Collections.ObjectModel;
using Chess.Moves;
using Chess.Web_Services.PlayerService;

namespace Chess.Pieces
{
    public class King : ChessPieceViewModel
    {
        public override string ImageSource => "../ChessPiecesIMG/" + (IsBlack ? "Black" : "White") + "King" + ".png";
        protected override Movement GetMovementStrategy()
        {
            return new KingMovement(this);
        }

        public King(ChessPiece piece) : base(piece)
        {

        }

        public King()
        {
            Type = ChessPieceEnum.King;
        }


        protected override void Move(Field targetField)
        {

            if (targetField.Column - Column == -2 && targetField.Row == Row)
            {
                new KingMovement(Formation.Pieces,this).TryMoveLeftRookToRightOfKing(targetField);
            }
            else if (targetField.Column - Column == 2 && targetField.Row == Row)
            {
                new KingMovement(Formation.Pieces, this).TryMoveRightRookToLeftOfKing(targetField);
            }
            base.Move(targetField);     
        }
    }
}
