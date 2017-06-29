using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chess;
using Chess.Pieces;
using NUnit.Framework;

namespace Unittests.Movements
{
    [TestFixture]
    class KingMovementTest
    {
        [TestCase(3, 3, true, 2, 2, true)]
        [TestCase(3, 3, true, 4, 3, true)]
        [TestCase(3, 3, true, 6, 0, false)]
        [TestCase(3, 3, true, 0, 0, false)]

        [SetUp]
        public void InitState()
        {
            Formation.Pieces = new ObservableCollection<ChessPieceViewModel>();
        }

        public void IsMovementValid(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var king = new King
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(king.CanMove(targetField), Is.EqualTo(result));
        }

        [TestCase(3, 3, true, 2, 2, true)]
        public void IsSomethingInTheWay(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var pawn = new Pawn
            {
                Row = 2,
                Column = 2,
                IsBlack = isBlack
            };

            Formation.Pieces.Add(pawn);

            var king = new King
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(king.CanMove(targetField, Formation.Pieces), Is.EqualTo(result));
        }

        [TestCase(3, 3, true, 2, 2, true)]
        public void EatOpponent(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var pawn = new Pawn
            {
                Row = 2,
                Column = 2,
                IsBlack = !isBlack
            };

            Formation.Pieces.Add(pawn);

            var king = new King
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(king.CanMove(targetField), Is.EqualTo(result));
            Assert.That(Formation.Pieces.Contains(pawn));

            king.TryConsumeAtTargetField(targetField);

            Assert.That(Formation.Pieces.Contains(pawn), Is.False);
            Assert.That(king.Column, Is.EqualTo(targetField.Column));
            Assert.That(king.Row, Is.EqualTo(targetField.Row));
        }

        //[Test]
        //public void SimpleRochadeTest()
        //{
        //    DoRochade(new List<ChessPieceViewModel>(), true);
        //}

        [Test]
        public void RochadeChessTest()
        {
            var others = new List<ChessPieceViewModel>();
            var rook = new Rook
            {
                Row = 1,
                Column = 6,
                IsBlack = true,
                FirstMove = true
            };
            others.Add(rook);
            //DoRochade(others, false);
        }

        [Test]
        public void RochadeIsMyPieceInTheWay()
        {
            var others = new List<ChessPieceViewModel>();

            var rook = new Rook
            {
                Row = 0,
                Column = 7,
                IsBlack = true,
                FirstMove = true
            };

            var knight = new Knight
            {
                Row = 0,
                Column = 6,
                IsBlack = true
            };

            others.Add(rook);
            others.Add(knight);
            DoRochade(others, false);
        }

        [Test]
        public void RochadeIsEnemyInTheWay()
        {
            var others = new List<ChessPieceViewModel>();

            var rook = new Rook
            {
                Row = 0,
                Column = 7,
                IsBlack = true
            };

            var knight = new Knight
            {
                Row = 0,
                Column = 6,
                IsBlack = !true
            };

            others.Add(rook);
            others.Add(knight);
            //DoRochade(others, false);
        }

        //Rochade Test
        public void DoRochade(List<ChessPieceViewModel> otherPieces, bool result)
        {
            otherPieces.ForEach(Formation.Pieces.Add);

            var rook = new Rook
            {
                Row = 0,
                Column = 7,
                IsBlack = false,
                FirstMove = true
            };

            var king = new King
            {
                Row = 0,
                Column = 4,
                IsBlack = false,
                FirstMove = true
            };

            Formation.Pieces.Add(rook);
            Formation.Pieces.Add(king);

            var targetField = new Field(0, 6);
            Assert.That(king.CanMove(targetField), Is.EqualTo(result));
            Assert.That(king.NoChess(targetField), Is.EqualTo(result));
            if (result)
            {
                king.TryMove(targetField,Formation.Pieces);
                Assert.That(king.Column, Is.EqualTo(6));
                Assert.That(rook.Column, Is.EqualTo(5));
            }
            else
            {
                Assert.That(king.Column, Is.EqualTo(4));
                Assert.That(rook.Column, Is.EqualTo(7));
            }
        }

    }
}
