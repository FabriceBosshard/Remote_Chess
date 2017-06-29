
using System;
using System.ComponentModel;
using Chess.Pieces;
using Chess.Web_Services.PlayerService;

namespace Chess.Factory
{
    public class ChessPieceFactory
    {
        public ChessPieceViewModel Create(ChessPiece piece)
        {
            ChessPieceEnum switchValue;
            Enum.TryParse(piece.Type, out switchValue);

            switch (switchValue)
            {
                case ChessPieceEnum.Pawn:
                    return new Pawn(piece);
                case ChessPieceEnum.Bishop:
                    return new Bishop(piece);
                case ChessPieceEnum.Rook:
                    return new Rook(piece);
                case ChessPieceEnum.King:
                    return new King(piece);
                case ChessPieceEnum.Queen:
                    return new Queen(piece);
                case ChessPieceEnum.Knight:
                    return new Knight(piece);
                default:
                    throw new InvalidEnumArgumentException($@"The type {piece.Type} is not supported.");
            }
        }       
    }
}