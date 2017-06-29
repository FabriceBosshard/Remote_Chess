using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chess;
using Chess.Pieces;
using NUnit.Framework;

namespace Unittests.Movements
{
    [TestFixture]
    class PawnMovementTest
    {
        

        [SetUp]
        public void InitState()
        {
            Formation.Pieces = new ObservableCollection<ChessPieceViewModel>();
        }

        [TestCase(1, 0, true, 2, 0, true)]
        [TestCase(1, 1, true, 2, 1, true)]
        [TestCase(1, 0, true, 3, 0, true)]
        [TestCase(2, 2, true, 3, 7, false)]
        [TestCase(1, 1, true, 8, 8, false)]
        public void IsMovementValid(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var pawn = new Pawn
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(pawn.CanMove(targetField), Is.EqualTo(result));
        }

        [TestCase(1, 0, true, 2, 0, false)]
        public void IsSomethingInTheWay(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var rook = new Rook
            {
                Row = 2,
                Column = 0,
                IsBlack = isBlack
            };

            Formation.Pieces.Add(rook);

            var pawn = new Pawn
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(pawn.CanMove(targetField), Is.EqualTo(result));
        }

        [TestCase(1, 0, true, 2, 0, false)]
        public void EatOpponent(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var bishop = new Bishop
            {
                Row = destinationRow,
                Column = destinationColumn,
                IsBlack = !isBlack
            };
            
            Formation.Pieces.Add(bishop);

            var pawn = new Pawn
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(pawn.CanMove(targetField), Is.EqualTo(result));
            Assert.That(Formation.Pieces.Contains(bishop));

            pawn.TryConsumeAtTargetField(targetField);

            Assert.That(Formation.Pieces.Contains(bishop), Is.False);
            Assert.That(pawn.Column, Is.EqualTo(targetField.Column));
            Assert.That(pawn.Row, Is.EqualTo(targetField.Row));

        }

        [TestCase( 4,0,false)]
        [TestCase(5, 0, true)]
        public void SimpleEnPassantTest(int BishRow, int BishCol,bool result)
        {
            var others = new List<ChessPieceViewModel>();
            var whiteKing = new King
            {
                Row = 0,
                Column = 4,
                IsBlack = false
            };
            var blackKing = new King
            {
                Row = 7,
                Column = 4,
                IsBlack = true
            };

            var blackBishop = new Bishop()
            {
                Row = BishRow,
                Column = BishCol,
                IsBlack = true
            };
            others.Add(whiteKing);
            others.Add(blackKing);
            others.Add(blackBishop);
            DoEnPassant(others, result);
        }

        //EnPassant Test
        public void DoEnPassant(List<ChessPieceViewModel> otherPieces, bool result)
        {
            otherPieces.ForEach(Formation.Pieces.Add);

            var pawn = new Pawn
            {
                Row = 3,
                Column = 5,
                IsBlack = false,
                FirstMove = true
            };

            var enemyPawn = new Pawn
            {
                Row = 3,
                Column = 6,
                IsBlack = true,
                IsEnPassantEnabled = true,
                
            };

            Formation.Pieces.Add(pawn);
            Formation.Pieces.Add(enemyPawn);

            var myTargetField = new Field(2, 6);
            Assert.That(pawn.TryMoveWithoutStateEffection(myTargetField, Formation.Pieces), Is.EqualTo(result));

            //Assert.That(Formation.Pieces.Contains(enemyPawn), Is.EqualTo(!result));
        }

        [TestCase(3,6,true)]
        [TestCase(4, 6,false)]
        public void IsEnPassantEnabledAfterDoubleMove(int targetRow, int targetCol,bool result)
        {
            var pawn = new Pawn
            {
                Row = 1,
                Column = 6,
                IsBlack = true,
                FirstMove = true
            };

            Formation.Pieces.Add(new King
            {
                Row = 0,
                Column = 4,
                IsBlack = false
            });
            Formation.Pieces.Add( new King
            {
                Row = 7,
                Column = 4,
                IsBlack = true
            });

            Formation.Pieces.Add(pawn);

            Field field = new Field(targetRow,targetCol);

            pawn.TryMoveWithoutStateEffection(field,Formation.Pieces);

            Assert.That(pawn.IsEnPassantEnabled == result);
        }
    }
}
