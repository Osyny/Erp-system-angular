using Erp_ang2.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Helpers
{
    public class ManagerRoles
    {
        private UserManager<ApplicationUser> userManager;

        public ManagerRoles(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public string GetRole(ApplicationUser user)
        {
            string userRole = "";

            var uRoles = this.userManager.GetRolesAsync(user).Result;

            if (uRoles.Contains("Admin"))
            {
                userRole = "Admin";
            }
            else if (uRoles.Contains("User"))
            {
                userRole = "User";
            }
            return userRole;
        }
    }
}
