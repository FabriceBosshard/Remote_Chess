using Chess;
using Chess.Pieces;
using NUnit.Framework;

namespace Unittests.Movements
{
    [TestFixture]
    class RookMovementTest
    {
        [TestCase(1, 0, true, 2, 0, true)]
        [TestCase(0, 0, true, 2, 1, false)]
        [TestCase(4, 6, true, 7, 6, true)]
        [TestCase(3, 2, true, 7, 6, false)]

        public void IsMovementValid(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var rook = new Rook
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(rook.CanMove(targetField), Is.EqualTo(result));
        }



        [TestCase(1, 0, true, 2, 0, true)]
        public void IsSomethingInTheWay(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var pawn = new Pawn
            {
                Row = 2,
                Column = 0,
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

        [TestCase(1, 0, true, 2, 0, true)]
        public void EatOpponent(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var pawn = new Pawn
            {
                Row = 2,
                Column = 0,
                IsBlack = !isBlack
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
            Assert.That(Formation.Pieces.Contains(pawn));

            rook.TryConsumeAtTargetField(targetField);

            Assert.That(Formation.Pieces.Contains(pawn), Is.False);
            Assert.That(rook.Column, Is.EqualTo(targetField.Column));
            Assert.That(rook.Row, Is.EqualTo(targetField.Row));
        }

    }
}
