using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain;
using Main.Utils;

namespace Main.Pages
{
    public class ChatModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public ChatModel(DAL.AppDbContext context) {
            _context = context;
        }

        public User User;

        public IActionResult OnGet() {
            User = this.GetCurrentUser(_context);

            if (User == null) return Redirect("/Login?b=" + Request.Path);

            return null;
        }
    }
}