using System.ComponentModel;
using Chess.Factory;
using Chess.Pieces;
using Chess.Web_Services.PlayerService;
using NUnit.Framework;

namespace Unittests.Factory
{
    [TestFixture]
    public class ChessPieceFactoryTest
    {
        [TestCase("Pawn", ChessPieceEnum.Pawn)]
        [TestCase("Bishop", ChessPieceEnum.Bishop)]
        [TestCase("Rook", ChessPieceEnum.Rook)]
        [TestCase("King", ChessPieceEnum.King)]
        [TestCase("Queen", ChessPieceEnum.Queen)]
        [TestCase("Knight", ChessPieceEnum.Knight)]
        public void Given_a_dto_and_called_create_then_return_correct_object(string inputType, ChessPieceEnum expectedType)
        {
            var rookDto = new ChessPiece
            {
                Type = inputType
            };

            ChessPieceFactory unitundertest = new ChessPieceFactory();
            var result = unitundertest.Create(rookDto);
            Assert.That(result.Type, Is.EqualTo(expectedType));
        }

        [Test]
        public void Given_a_dto_with_unknown_type_and_called_create_then_exception_is_thrown3()
        {
            var rookDto = new ChessPiece
            {
                Type = "foo"
            };
            ChessPieceFactory unitundertest = new ChessPieceFactory();
            Assert.Throws<InvalidEnumArgumentException>(() => unitundertest.Create(rookDto));
        }
    }
}
