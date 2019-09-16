using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Domain;
using Microsoft.AspNetCore.SignalR;

namespace Game.Matchmaking {
    public static class Handler {
        static Dictionary<string, Waiting> _waiting = new Dictionary<string, Waiting>();
        static Dictionary<string, Playing> _playing = new Dictionary<string, Playing>();
        static Dictionary<string, string> _connectionToUserMap = new Dictionary<string, string>();
        static Dictionary<string, string> _userToConnectionMap = new Dictionary<string, string>();

        static string GetKey(Guid guid) => guid.ToString();

        public static Domain.Game TryGetWaitingGame(Guid host) {
            _waiting.TryGetValue(GetKey(host), out Waiting g);
            return g.Game;
        }

        public static bool IsWaiting(User user) => _waiting.ContainsKey(user.Guid.ToString());

        public static void AddWaiting(IClientProxy client, string connectionId, Domain.Game game) {
            var owner = game.Player1.Guid;
            string key = GetKey(owner);

            _waiting[key] = new Waiting() {
                Game = game,
                MatchInfo = new Domain.Matchmaking.MatchInfo() { Name = game.GameName, OwnerId = owner, MatchId = game.GameGuid },
                Client = client,
            };
            _connectionToUserMap[connectionId] = key;
            _userToConnectionMap[key] = connectionId;
        }

        public static void RemoveWaiting(Guid host) => RemoveWaiting(GetKey(host));
        public static void RemoveWaiting(string guid) => _waiting.Remove(guid);

        public static Playing TryJoinWaiting(AppDbContext context, Guid host, Guid player, IClientProxy client, string playerConnectionId) {
            var hostkey = GetKey(host);
            var playerkey = GetKey(player);

            _waiting.TryGetValue(hostkey, out Waiting waiting);
            if (waiting == null) return null;

            var playing = new Playing() {
                Game = waiting.Game,
                MatchInfo = waiting.MatchInfo,
                Players = new List<Player>() {
                    new Player() {
                        ConnectionId = _userToConnectionMap[hostkey],
                        Client = waiting.Client,
                        User = context.Users.First(u => u.Guid.Equals(host)),
                    },
                    new Player() {
                        ConnectionId = playerConnectionId,
                        Client = client,
                        User = context.Users.First(u => u.Guid.Equals(player)),
                    },
                }
            };

            _playing[hostkey] = playing;
            _playing[playerkey] = playing;
            
            RemoveWaiting(hostkey);

            return playing;
        }

        public static Playing GetGame(Guid user) => _playing[GetKey(user)];

        public static bool TryRemoveWaiting(string connectionId) {
            if (_connectionToUserMap.TryGetValue(connectionId, out string guid)) {
                RemoveWaiting(guid);
                _connectionToUserMap.Remove(connectionId);
                _userToConnectionMap.Remove(guid);
                return true;
            }
            return false;
        }

        public static List<Domain.Matchmaking.MatchInfo> GetWaiting() => _waiting.Values.Select(w => w.MatchInfo).ToList();
        
    }
}
