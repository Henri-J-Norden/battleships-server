using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain;
using Main.Utils;

namespace Võitluslaevad___server.Pages {
    public class IndexModel : PageModel {

        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context) {
            _context = context;
        }

        public User User;

        public void OnGet() {
            User = this.GetCurrentUser(_context);
        }


    }
}
