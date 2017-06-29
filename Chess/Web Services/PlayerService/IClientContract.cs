//PLAYER
//id
//Name
//farbe
//currentgameid
//POST Json string name
//172.16.0.27:8080/WebService/webservice/createplayer

using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Chess.Web_Services.PlayerService
{
    [ServiceContract]
    public interface IClientContract
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "/player/{id}")]
        Player GetPlayer(string id);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/createplayer/")]
        Player CreatePlayer(Player newPlayer);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        UriTemplate = "/showstate/{gameid}")]
        GameState ShowState(string gameid);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/startgame/")]
        Player StartGame(Player newPlayer);

        [OperationContract]
        [WebInvoke(Method = "POST",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           UriTemplate = "setgamestate/{gameid}")]
        bool SetGameState(string gameid, GameState state);
    }

    [DataContract]
    public class Player
    {
        [DataMember]
        public int CurrentGame { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsBlack { get; set; }
    }

    [DataContract]
    public class GameState
    {

        public GameState()
        {
            Id = Guid.NewGuid().ToString();
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public bool IsBlack { get; set; }

        [DataMember]
        public ChessPiece[] Pieces { get; set; }
    }

    [DataContract]
    public class ChessPiece
    {
        [DataMember]
        public bool IsBlack { get; set; }
        [DataMember]
        public bool FirstMove = true;
        [DataMember]
        public bool IsEnPassantEnabled;
        [DataMember]
        public bool IsSelected { get; set; }
        [DataMember]
        public int Row { get; set; }
        [DataMember]
        public int Column { get; set; }
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Type { get; set; }
    }
}

