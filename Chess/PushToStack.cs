using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Pieces;

namespace Chess
{
    class PushToStack
    {
        private ChessPieceViewModel piece;
        public PushToStack(Field target, int column, int row, ChessPieceViewModel piece)
        {
            this.piece = piece;
            string MtargetR= getValueRow(target.Row);
            string MtargetM = getValueCol(target.Column);
            string Mcolumn = getValueCol(column);
            string Mrow = getValueRow(row);
            Chessboard.stackMsg.Add(finalizeString(MtargetM,MtargetR,Mcolumn,Mrow));
        }

        public PushToStack(Field targetField, int column, int row,int RCol, int RTCol, ChessPieceViewModel piece)
        {
            this.piece = piece;
            string KRow = getValueRow(row);
            string KCol = getValueCol(column);
            string KTRow = getValueRow(targetField.Row);
            string KTCol = getValueCol(targetField.Column);
            string RTColumn = getValueCol(RTCol);
            string RColumn = getValueCol(RCol);

            Chessboard.stackMsg.Add(finalizeString(KRow,KCol,KTRow,KTCol,RTColumn,RColumn));
        }

        public PushToStack(string msg)
        {
            Chessboard.stackMsg.Add(msg);
        }

        private string getValueRow(int row)
        {
            switch (row)
            {
                case 0:
                    return "8";
                   
                case 1:
                    return "7";
                   
                case 2:
                    return "6";
                   
                case 3:
                    return "5";
                   
                case 4:
                    return "4";
                    
                case 5:
                    return "3";
                    
                case 6:
                    return "2";
                case 7:
                    return "1";
                    
            }
            return "";
        }
        private string getValueCol(int col)
        {
            switch (col)
            {
                case 0:
                    return "A";

                case 1:
                    return "B";

                case 2:
                    return "C";

                case 3:
                    return "D";

                case 4:
                    return "E";

                case 5:
                    return "F";

                case 6:
                    return "G";
                case 7:
                    return "H";

            }
            return "";
        }

        private string finalizeString(string a,string b, string column, string row)
        {
            return piece.Type + (Chessboard._activePlayer.IsBlack ? " Black:  " : " White: ")+column +row+" - "+a+b;
        }

        private string finalizeString(string a, string b, string column, string row,string c,string d)
        {
            return (Chessboard._activePlayer.IsBlack ? "Black  " : "White ") + piece.Type + ": " + row + column +
                   " - " + b + a + " | " + ChessPieceEnum.Rook + ": " + c + column + " - " + d + column;
        }
    }
}
