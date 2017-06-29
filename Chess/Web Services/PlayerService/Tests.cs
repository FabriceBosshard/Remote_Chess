using NUnit.Framework;

namespace Chess.Web_Services.PlayerService
{
    [TestFixture]
    public class TestClient
    {
        
        [Test]
        public void AddNewPlayer()
        {
            var spieler = new Player {Name = "Bratwurst"};
            var client = new Client();
            client.Open();
            spieler = client.CreatePlayer(spieler);

            Assert.That(spieler.Name, Is.EqualTo("Bratwurst"));
            Assert.That(spieler.IsBlack, Is.EqualTo(false));
        }

        [Test]
        public void StartGame()
        {
            var client = new Client();
            client.Open();
            var spieler1 = client.CreatePlayer(new Player{ Name = "Toshiki"});
            var spieler2 = client.CreatePlayer(new Player() {Name = "Torben"});

            spieler1 = client.StartGame(spieler1);
            spieler2 = client.StartGame(spieler2);

            Assert.That(spieler1.CurrentGame, Is.EqualTo(spieler2.CurrentGame));
            Assert.That(spieler1.IsBlack, Is.Not.EqualTo(spieler2.IsBlack));
        }

        [Test]
        public void PassGameTest()
        {
            var client = new Client();
            client.Open();
            var spieler1 = client.CreatePlayer(new Player { Name = "Toshiki" });
            var spieler2 = client.CreatePlayer(new Player() { Name = "Torben" });

            spieler1 = client.StartGame(spieler1);
            spieler2 = client.StartGame(spieler2);

            var gameId = spieler1.CurrentGame;

            Assert.That(spieler1.CurrentGame, Is.EqualTo(spieler2.CurrentGame));
            Assert.That(spieler1.IsBlack, Is.Not.EqualTo(spieler2.IsBlack));

            var state = new GameState { Id = gameId.ToString(), IsBlack = false };

            var success = client.SetGameState(spieler1.CurrentGame.ToString(), state);
            Assert.That(success, Is.EqualTo(true));

            var result = client.ShowState(gameId.ToString());

            Assert.That(result.IsBlack, Is.EqualTo(spieler1.IsBlack));
            Assert.That(result.Id, Is.EqualTo(state.Id));
        }
    }
}