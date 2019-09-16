using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using Microsoft.AspNetCore.SignalR;
using Main.Utils;


namespace Main.Hubs {
    public class ChatHub : DbHub {

        public ChatHub(AppDbContext context) : base(context) { }

        public async Task SendMessage(string user, string message) {
            var User = UserUtils.GetCurrentUser(_context, user);

            await Clients.All.SendAsync("ReceiveMessage", User.UserName, message);
        }
    }
}
