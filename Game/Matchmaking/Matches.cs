using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using Microsoft.AspNetCore.SignalR;

namespace Game.Matchmaking {
    internal class Waiting {
        public Domain.Game Game;
        public Domain.Matchmaking.MatchInfo MatchInfo;
        public IClientProxy Client;
    }

    public class Playing {
        public Domain.Game Game;
        public Domain.Matchmaking.MatchInfo MatchInfo;
        public Guid GameGuid => Game.GameGuid;
        public string GameId => GameGuid.ToString();
        public List<Player> Players;

        public Player GetOtherPlayer(Guid player) => Players.First(p => !p.User.Guid.Equals(player));
    }

    public class Player {
        public IClientProxy Client;
        public string ConnectionId;
        public User User;

    }
}
