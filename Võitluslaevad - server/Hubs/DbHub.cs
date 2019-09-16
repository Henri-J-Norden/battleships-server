using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.SignalR;

namespace Main.Hubs {
    public class DbHub : Hub {
        public readonly AppDbContext _context;

        public DbHub(AppDbContext context) {
            _context = context;
        }
        
    }
}
