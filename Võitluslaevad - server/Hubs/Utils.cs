using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using Microsoft.AspNetCore.SignalR;

namespace Main.Hubs {
    public static class Utils {

        public static User GetUser(this DbHub dbHub, string guid) {
            Guid g = Guid.Parse(guid);
            return dbHub._context.Users.FirstOrDefault(u => u.Guid == g);
        }

        public static async Task<bool> InvalidUser(this DbHub dbHub, User u) {
            if (u == null) await dbHub.Clients.Caller.SendAsync("Log", "Invalid GUID!");
            return u == null;
        }
    }
}
