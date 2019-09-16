using System;
using System.Linq;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Main.Utils {
    public static class UserUtils {
        public static User GetCurrentUser(DAL.AppDbContext context, string guid) {
            if (!String.IsNullOrEmpty(guid)) {
                var _guid = Guid.Parse(guid);

                return context.Users.First(u => u.Guid == _guid);
            }

            return null;
        }

        public static User GetCurrentUser(this PageModel pm, DAL.AppDbContext context) {
            string val;
            pm.Request.Cookies.TryGetValue("guid", out val);

            return GetCurrentUser(context, val);
        }

    }
}
