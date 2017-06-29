using Chess;
using Chess.Pieces;
using NUnit.Framework;

namespace Unittests.Movements
{
    [TestFixture]
    class BishopMovementTest
    {
        [TestCase(1, 1, true, 0, 0, true)]
        [TestCase(3, 3, true, 6, 0, true)]
        [TestCase(3, 3, true, 3, 7, false)]
        [TestCase(3, 3, true, 0, 3, false)]

        public void IsMovementValid(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var bishop = new Bishop
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(bishop.CanMove(targetField), Is.EqualTo(result));
        }

        [TestCase(1, 1, true, 0, 0, true)]
        public void IsSomethingInTheWay(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var rook = new Rook
            {
                Row = 0,
                Column = 0,
                IsBlack = isBlack
            };

            Formation.Pieces.Add(rook);

            var bishop = new Bishop
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(bishop.CanMove(targetField), Is.EqualTo(result));
        }

        [TestCase(1, 1, true, 0, 0, true)]
        public void EatOpponent(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var rook = new Rook
            {
                Row = 0,
                Column = 0,
                IsBlack = !isBlack
            };

            Formation.Pieces.Add(rook);

            var bishop = new Bishop
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(bishop.CanMove(targetField), Is.EqualTo(result));
            Assert.That(Formation.Pieces.Contains(rook));

            bishop.TryConsumeAtTargetField(targetField);

            Assert.That(Formation.Pieces.Contains(rook), Is.False);
            Assert.That(bishop.Column, Is.EqualTo(targetField.Column));
            Assert.That(bishop.Row, Is.EqualTo(targetField.Row));
        }

    }
}
