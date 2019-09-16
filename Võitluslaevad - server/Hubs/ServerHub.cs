using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using Game;
using Game.Matchmaking;
using Microsoft.AspNetCore.SignalR;

namespace Main.Hubs {
    public partial class ServerHub : DbHub {

        public static Dictionary<string, string> UserToConnectionMap = new Dictionary<string, string>();
        public static Dictionary<string, User> ConnectionToUserMap = new Dictionary<string, User>();


        public ServerHub(AppDbContext context) : base(context) { }

        async Task ExecLogin(User user, string connectionId) {
            UserToConnectionMap[user.Guid.ToString()] = connectionId;
            ConnectionToUserMap[connectionId] = user;

            await Clients.Caller.SendAsync("LoginResult", "Success", user);
        }

        void Disconnect(string connectionId) {
            if (ConnectionToUserMap.TryGetValue(connectionId, out User user)) {
                ConnectionToUserMap.Remove(connectionId);
                UserToConnectionMap.Remove(user.Guid.ToString());
            }
        }

        public async Task Login(string username, string password) {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null) {
                await Clients.Caller.SendAsync("LoginResult", "Invalid username!", null);
                return;
            }

            if (user.CheckPassword(password)) {
                ExecLogin(user, Context.ConnectionId);
            } else {
                await Clients.Caller.SendAsync("LoginResult", "Invalid password", null);
            }
        }

        public async Task LoginGuid(string guid) {
            ExecLogin(this.GetUser(guid), Context.ConnectionId);
        }

        public async Task Logout(string guid) {
            if (UserToConnectionMap.TryGetValue(guid, out var connectionId)) {
                Disconnect(connectionId);
            }
        }

        public async Task Register(string username, string password) {
            if (_context.Users.FirstOrDefault(u => u.UserName == username) != null) {
                await Clients.Caller.SendAsync("RegistrationResult", "Username taken!");
                return;
            }
            if (password.Length < 8) {
                await Clients.Caller.SendAsync("RegistrationResult", "The password must be longer than 8 characters!");
                return;
            }

            var user = new User();
            user.UserName = username;
            user.Password = password;
            _context.Users.Add(user);
            _context.SaveChanges();

            await Clients.Caller.SendAsync("RegistrationResult", "Success");
            ExecLogin(user, Context.ConnectionId);

        }

        static IClientProxy GetOtherPlayerClient(IHubCallerClients Clients, Guid userId) {
            var otherPlayer = Handler.GetGame(userId).GetOtherPlayer(userId);
            return Clients.Client(otherPlayer.ConnectionId);
        }

        public async Task GameBoard(Guid userId, Newtonsoft.Json.Linq.JObject board) {
            await GetOtherPlayerClient(Clients, userId).SendAsync("GameBoard", board);
        }

        public async Task Move(Guid userId, Move move) {
            await GetOtherPlayerClient(Clients, userId).SendAsync("Move", move);
        }


        public override Task OnDisconnectedAsync(Exception exception) {
            if (Handler.TryRemoveWaiting(Context.ConnectionId)) { // matchmaking terminated

            }

            Disconnect(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
