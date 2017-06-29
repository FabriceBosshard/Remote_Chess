using System.Linq;
using Chess;
using Chess.Moves;
using Chess.Pieces;
using NUnit.Framework;

namespace Unittests.Check_MovementSimulator
{
    [TestFixture]
    class MovementSimulatorTest
    {
        private Formation Fr;
        private King _king;
        private MoveSimulator unitundertest;

        [SetUp]
        public void initState()
        {
            Fr = new Formation();
            Fr.Initialize();

            _king = new King();

            foreach (var piece in Formation.Pieces)
            {
                if (!piece.IsBlack || piece.Type != ChessPieceEnum.King) continue;
                _king = piece as King;
                break;
            }
            unitundertest = new MoveSimulator(_king);
        }

        [Test]
        public void Copy_State_and_Check_for_all_Pieces()
        {                                             
            Assert.True(unitundertest._tempList.Count.Equals(Formation.Pieces.Count));
        }

        [Test]
        public void Get_the_KingUnderCheck()
        {
            Assert.True(unitundertest._tempList.Any(p=> _king != null && p.Row == _king.Row && p.Column == _king.Column));           
        }

        [TestCase(2,1,0,3,true)]
        [TestCase(2, 1, 7, 1, true)]
        public void Test_For_imaginary_move(int sourceRow, int sourceColumn,int targetRow, int targetColumn,bool result)
        {            
            Field targetField = new Field()
            {
                Column = targetColumn,
                Row = targetRow
            };

            ChessPieceViewModel piece = new Queen() {Column = sourceColumn,Row = sourceRow};
            Formation.Pieces.Add(piece);

            unitundertest = new MoveSimulator(piece,targetField);

            Assert.That(unitundertest._tempList.Any(p=> p.Type == ChessPieceEnum.Queen && p.Row ==targetRow&& p.Column == targetColumn ), Is.EqualTo(result));
        }

        [TestCase(2,2)]
        [TestCase(5,5)]
        [TestCase(5, 8)]
        public void Get_the_tempPiece(int targetColumn,int targetRow)
        {
            ChessPieceViewModel piece = new Queen() {Column = 4,Row = 4};

            Field targetField = new Field()
            {
                Column = targetColumn,
                Row = targetRow
            };
            Formation.Pieces.Add(piece);
            unitundertest = new MoveSimulator(piece, targetField);
            var x = unitundertest._tempPiece;
            Assert.True(x.Row == targetRow && x.Column == targetColumn && x.Type == piece.Type && x.IsBlack == piece.IsBlack);
        }     
    }
}
