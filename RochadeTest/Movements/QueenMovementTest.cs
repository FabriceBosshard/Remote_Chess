using Chess;
using Chess.Pieces;
using NUnit.Framework;

namespace Unittests.Movements
{
    [TestFixture]
    class QueenMovementTest
    {
        [TestCase(1, 0, true, 7, 6, true)]
        [TestCase(1, 1, true, 1, 7, true)]
        [TestCase(1, 1, true, 3, 7, false)]
        [TestCase(1, 1, true, 7, 2, false)]

        public void IsMovementValid(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var queen = new Queen
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(queen.CanMove(targetField), Is.EqualTo(result));
        }

        [TestCase(1, 0, true, 1, 6, false)]
        [TestCase(1, 0, true, 1, 7, false)]
        [TestCase(1, 0, true, 1, 5, false)]
        [TestCase(1, 0, true, 1, 4, true)]
        [TestCase(1, 0, false, 1, 6, false)]
        [TestCase(1, 0, false, 1, 7, false)]
        [TestCase(1, 0, false, 1, 5, false)]
        [TestCase(1, 0, false, 1, 4, true)]
        public void IsSomethingInTheWay(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var pawn = new Pawn
            {
                Row = 1,
                Column = 4,
                IsBlack = isBlack
            };

            Formation.Pieces.Add(pawn);

            var rook = new Rook
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(rook.CanMove(targetField), Is.EqualTo(result));
        }

        [TestCase(1, 0, true, 7, 6, true)]
        public void EatOpponent(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            //Arrange
            var pawn = new Pawn
            {
                Row = destinationRow,
                Column = destinationColumn,
                IsBlack = !isBlack
            };

            Formation.Pieces.Add(pawn);

            var queen = new Queen
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(queen.CanMove(targetField), Is.EqualTo(result));
            Assert.That(Formation.Pieces.Contains(pawn));

            //act
            queen.TryConsumeAtTargetField(targetField);


            //assert
            Assert.That(Formation.Pieces.Contains(pawn), Is.False);
            Assert.That(queen.Column, Is.EqualTo(targetField.Column));
            Assert.That(queen.Row, Is.EqualTo(targetField.Row));

        }

    }
}
