using System.Runtime.Serialization;

namespace Chess.Pieces
{
    [DataContract]
    public partial class Field
    {
        [DataMember]
        public int Row;
        [DataMember]
        public int Column;
    }

    public partial class Field
    {
        public Field(int row, int col)
        {
            Row = row;
            Column = col;
        }
    }
}