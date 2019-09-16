using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace Main.Pages_Users
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User User { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (_context.Users.FirstOrDefault(u => u.UserName == User.UserName) != null) return BadRequest("Username taken!");

            //User.Guid = Guid.NewGuid();
            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            Response.Cookies.Append("guid", User.Guid.ToString());

            return Redirect("/");
        }
    }
}