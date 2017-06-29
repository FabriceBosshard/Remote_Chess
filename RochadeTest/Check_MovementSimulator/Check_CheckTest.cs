using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chess;
using Chess.Moves;
using Chess.Pieces;
using Chess.Web_Services.PlayerService;
using NUnit.Framework;

namespace Unittests.Check_MovementSimulator
{
    [TestFixture]
    class Check_CheckTest
    {
        private Player _activePlayer = new Player() {IsBlack = true};
        private King unitUnderTest;


        [SetUp]
        public void StateSetup()
        {
            Formation.Pieces = new ObservableCollection<ChessPieceViewModel>();
        }

        [TestCase(2, 5, true)]
        [TestCase(7, 0, true)]
        [TestCase(2, 7, false)]
        [TestCase(6, 0, false)]
        public void At_start_of_turn_is_king_in_check(int sourceRow, int sourceColumn, bool result)
        {
            Formation.Pieces.Add(new King() {Column = 3, Row = 4, IsBlack = true, Type = ChessPieceEnum.King});
            Formation.Pieces.Add(new Bishop()
            {
                Column = sourceColumn,
                Row = sourceRow,
                IsBlack = false,
                Type = ChessPieceEnum.Bishop
            });

            Assert.That(new MoveSimulator(_activePlayer).IsCheck, Is.EqualTo(result));

        }

        [TestCase(7, 5, true, 2)]
        [TestCase(3, 5, true, 2)]
        [TestCase(4, 5, false, 2)]
        [TestCase(5, 5, false, 2)]
        [TestCase(0,5,true,6)]
        [TestCase(4, 5, false, 6)]
        [TestCase(3,5,false,6)]
        public void King_would_be_in_check_while_Rochade(int sourceColumnAttacker,int sourceRowAttacker,bool result, int targetColumnKing)
        {
           initializeState(sourceColumnAttacker,sourceRowAttacker);

            var currentColumn = unitUnderTest.Column;
            var targetColumn = targetColumnKing;

            List<Field> fields = new List<Field>();

            if (targetColumn >= currentColumn)
            {
                for (int i = targetColumn; i > currentColumn; i--)
                {
                    fields.Add(new Field() { Column = i, Row = unitUnderTest.Row });
                }
            }
            else
            {
                for (int i = targetColumn; i < currentColumn; i++)
                {
                    fields.Add(new Field() { Column = i, Row = unitUnderTest.Row });
                }
            }

            Assert.That(new MoveSimulator(unitUnderTest).CheckOnMultipleFields(fields), Is.EqualTo(result));

        }

        public void initializeState(int sourceColumnAttacker, int sourceRowAttacker)
        {
            unitUnderTest = new King()
            {
                Column = 4,
                Row = 7,
                IsBlack = true,
                FirstMove = true,
                Type = ChessPieceEnum.King
            };
            Formation.Pieces.Add(unitUnderTest);
            Formation.Pieces.Add(new Rook()
            {
                Column = 0,
                Row = 7,
                IsBlack = true,
                FirstMove = true,
                Type = ChessPieceEnum.Rook
            });
            Formation.Pieces.Add(new Rook()
            {
                Column = 7,
                Row = 7,
                IsBlack = true,
                FirstMove = true,
                Type = ChessPieceEnum.Rook
            });
            Formation.Pieces.Add(new Bishop()
            {
                Column = sourceColumnAttacker,
                Row = sourceRowAttacker,
                IsBlack = false,
                Type = ChessPieceEnum.Bishop
            });
        }

        [TestCase(2,1,false,4,3)]
        [TestCase(4,6,false,4,3)]
        [TestCase(3, 0, false, 5,2)]
        [TestCase(5,5, false, 5,2)]
        [TestCase(2, 1, true, 4, 1)]
        [TestCase(4, 6, true, 4, 1)]
        public void Move_figure_in_path_of_checked_king(int sourceColumn, int sourceRow,bool result,int targetColumn,int targetRow)
        {
            Formation.Pieces.Add(new King()
            {
                Column = 3,
                Row = 4,
                IsBlack = true,
                Type = ChessPieceEnum.King
            });
            Queen a = new Queen()
            {
                Column = sourceColumn,
                Row = sourceRow,
                IsBlack = true,
                Type = ChessPieceEnum.Queen
            };
            Formation.Pieces.Add(a);
            Formation.Pieces.Add(new Bishop()
            {
                Column = 5,
                Row = 2,
                IsBlack = false,
                Type = ChessPieceEnum.Bishop
            });
            Field targetField = new Field()
            {
                Column = targetColumn,
                Row = targetRow
            };

            Assert.That(new MoveSimulator(a,targetField).IsCheck,Is.EqualTo(result));
        }
    }
}
