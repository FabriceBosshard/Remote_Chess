using Chess;
using Chess.Pieces;
using Chess.Web_Services.PlayerService;
using NUnit.Framework;

namespace Unittests.Pieces
{
    [TestFixture]
    public class ChessPieceViewModelTest
    {
        [Test]
        public void UpdateChessPieceViewModelTest()
        {
            var unitundertest = new Rook();

            ChessPiece rookDTO = new ChessPiece
            {
                IsBlack = true,
                Column = 1234
            };

            unitundertest.UpdateChessPieceViewModel(rookDTO);

            Assert.That(unitundertest.IsBlack, Is.True);
            Assert.That(unitundertest.Column, Is.EqualTo(1234));
        }

        [Test]
        public void ToDataContractTest()
        {
           
            ChessPiece rookDTO = new ChessPiece
            {
                IsBlack = true,
                Column = 1234
            };

            var unitundertest = new Rook(rookDTO);

            var dataContractResult = unitundertest.ToDataContract();

            Assert.That(dataContractResult.IsBlack, Is.True);
            Assert.That(dataContractResult.Column, Is.EqualTo(1234));
        }
    }
}
