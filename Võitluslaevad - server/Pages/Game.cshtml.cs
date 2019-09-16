using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Main.Pages
{
    public class GameModel : PageModel
    {
        public IActionResult OnGet()
        {
            Request.Cookies.TryGetValue("guid", out string guid);

            if (String.IsNullOrEmpty(guid)) return Redirect("/Login?b=" + Request.Path);
            return Page();
        }
    }
}