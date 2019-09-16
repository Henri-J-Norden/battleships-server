using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.SignalR;
using Game.Matchmaking;
using Domain;

namespace Main.Hubs {
    public partial class ServerHub : DbHub {

        public async Task StartMatchmaking(Guid host, Domain.Game game) => Handler.AddWaiting(Clients.Caller, Context.ConnectionId, game);

        public async Task StopMatchmaking(Guid guid) => Handler.RemoveWaiting(guid);

        public async Task ListMatches() => Clients.Caller.SendAsync("MatchList", Handler.GetWaiting());

        public async Task JoinMatch(Guid host, Guid player) {
            var g = Handler.TryJoinWaiting(_context, host, player, Clients.Caller, Context.ConnectionId);
            if (g == null) { // joining failed
                return;
            }

            foreach (var p in g.Players) await Groups.AddToGroupAsync(p.ConnectionId, g.GameId);
            
            for (int i = 0; i < 2; i++) {
                int j = (i + 1) % 2;
                Debug.WriteLine($"i: {i}; j: {j}");
                Clients.Client(g.Players[i].ConnectionId).SendAsync("MatchStart", g.GameGuid, g.Players[j].User);
                //g.Players[i].Client.SendAsync("MatchStart", g.GameGuid, g.Players[j].User);
            }
            

            
        }

        public async Task GetMatchData(Guid host) {
            Domain.Game g = Handler.TryGetWaitingGame(host);
            string msg = "";

            if (g == null) msg = "Match not found! (Try refreshing)";
            else if (g.Player2 != null) msg = "This match already has a second player.";

            await Clients.Caller.SendAsync("MatchData", msg, g);
        }


        
    }
}
