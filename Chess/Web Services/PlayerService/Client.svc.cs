using System.ServiceModel;

namespace Chess.Web_Services.PlayerService
{
    public class Client : ClientBase<IClientContract>, IClientContract
    {
        public Player GetPlayer(string id)
        {
            return Channel.GetPlayer(id);
        }

        public Player CreatePlayer(Player newPlayer)
        {
            return Channel.CreatePlayer(newPlayer);
        }

        public GameState ShowState(string gameid)
        {
            return Channel.ShowState(gameid);
        }

        public Player StartGame(Player newPlayer)
        {
            return Channel.StartGame(newPlayer);
        }

        public bool SetGameState(string gameid, GameState state)
        {
            return Channel.SetGameState(gameid, state);
        }
    }
}
