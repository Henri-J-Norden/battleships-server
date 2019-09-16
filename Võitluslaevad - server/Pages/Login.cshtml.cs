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
    public class LoginModel : PageModel
    {
        
        private readonly DAL.AppDbContext _context;

        public LoginModel(DAL.AppDbContext context) {
            _context = context;
        }

        public User User;

        public void OnGet()
        {
            User = this.GetCurrentUser(_context);
        }

        public IActionResult OnPost(string username, string password) {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null) {
                ViewData["usernameError"] = "User does not exist!";
                return null;
            }
            
            if (password == null || !user.CheckPassword(password)) {
                ViewData["passwordError"] = "Wrong password!";
                return null;
            }

            //Response.Cookies.Append("login", "1");
            Response.Cookies.Append("guid", user.Guid.ToString());
            
            if (Request.Query.ContainsKey("b")) {
                return Redirect(Request.Query["b"]);
            } else {
                return Redirect("/");
            }
        }
    }
}