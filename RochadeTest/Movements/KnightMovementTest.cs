using System.Linq;
using System.Runtime.Versioning;
using Chess;
using Chess.Pieces;
using NUnit.Framework;

namespace Unittests.Movements
{
    [TestFixture]
    class KnightMovementTest
    {
        [TestCase(1, 0, true, 3, 1, true)]
        [TestCase(5, 4, true, 4, 6, true)]
        [TestCase(2, 4, true, 1, 3, false)]
        [TestCase(0, 0, true, 7, 7, false)]

        public void IsMovementValid(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var knight = new Knight
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(knight.CanMove(targetField), Is.EqualTo(result));
        }

        [TestCase(1, 0, true, 3, 1, true)]
        public void EatOpponent(int sourceRow, int sourceColumn, bool isBlack, int destinationRow, int destinationColumn, bool result)
        {
            var pawn = new Pawn
            {
                Row = destinationRow,
                Column = destinationColumn,
                IsBlack = !isBlack
            };

            Formation.Pieces.Add(pawn);

            var knight = new Knight
            {
                Row = sourceRow,
                Column = sourceColumn,
                IsBlack = isBlack
            };
            var targetField = new Field(destinationRow, destinationColumn);
            Assert.That(knight.CanMove(targetField), Is.EqualTo(result));
            Assert.That(Formation.Pieces.Contains(pawn));

            knight.TryConsumeAtTargetField(targetField);

            Assert.That(Formation.Pieces.Contains(pawn), Is.False);
            Assert.That(knight.Column, Is.EqualTo(targetField.Column));
            Assert.That(knight.Row, Is.EqualTo(targetField.Row));
        }

    }
}
