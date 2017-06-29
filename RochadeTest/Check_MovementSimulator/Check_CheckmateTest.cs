using System;
using System.Collections.ObjectModel;
using Chess;
using Chess.Moves;
using Chess.Pieces;
using NUnit.Framework;

namespace Unittests.Check_MovementSimulator
{
    [TestFixture]
    class Check_CheckmateTest
    {
        private King unitUnderTest;
        

        [SetUp]
        public void StateSetup()
        {           
            Formation.Pieces = new ObservableCollection<ChessPieceViewModel>();
        }


        [TestCase(7,7,3,3,3,4,2,6,true)]
        [TestCase(0,0,2,2,2,3,4,3,true)]
        [TestCase(0, 0, 2, 2, 2, 3, 4,2, false)]
        [TestCase(5,5, 3, 3, 2, 2, 1, 1, false)]
        public void King_is_checkmate(int rowKing, int colKing, int rowAtt1, int colAtt1, int rowAtt2, int colAtt2, int rowQueen, int colQueen, object result)
        {
            initStateCheckmate(rowKing,colKing,rowAtt1,colAtt1,rowAtt2,colAtt2,rowQueen,colQueen);

            Assert.That(new MoveSimulator(unitUnderTest).IsCheckmate(), Is.EqualTo(result));
        }

        private void initStateCheckmate(int rowKing,int colKing,int rowAtt1,int colAtt1,int rowAtt2, int colAtt2,int rowQueen,int colQueen)
        {
            Formation.Pieces.Add(new Bishop()
            {
                Row = rowAtt1,
                Column = colAtt1,
                Type = ChessPieceEnum.Bishop,
                IsBlack = true
            });
            Formation.Pieces.Add(new Bishop()
            {
                Row = rowAtt2,
                Column = colAtt2,
                Type = ChessPieceEnum.Bishop,
                IsBlack = true
            });
            unitUnderTest = new King()
            {
                Row = rowKing,
                Column = colKing,
                Type = ChessPieceEnum.King,
                IsBlack = false
            };
            Formation.Pieces.Add(unitUnderTest);

            Formation.Pieces.Add(new Queen()
            {
                Row = rowQueen,
                Column = colQueen,
                Type = ChessPieceEnum.Queen,
                IsBlack = true
            });
        }
    }
}
