using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using Game;
using Microsoft.AspNetCore.SignalR;

namespace Main.Hubs {
    public class GameHub : DbHub {

        public GameHub(AppDbContext context) : base(context) { }

        public async Task Start(string guid) {
            User u = this.GetUser(guid);
            if (await this.InvalidUser(u)) return;
            
            await Clients.Caller.SendAsync("Update", "Starting game...");
            
            
            InstanceHandler.NewInstance(Clients.Caller, Context.ConnectionId, guid);
            
        }

        public async Task KeyPress(string guid, string keyCode) {
            Instance i = InstanceHandler.TryGetInstance(guid);
           
            if (i == null) {
                Clients.Caller.SendAsync("Log", "ERROR: Game instance not active!");
            } else {
                i.WriteChar((char)Convert.ToInt32(keyCode));
            }
        }


        public override Task OnDisconnectedAsync(Exception exception) {
            InstanceHandler.TryRemoveInstance(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
