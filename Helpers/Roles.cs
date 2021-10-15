using Erp_ang2.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Helpers
{
    public class Roles
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;

        public Roles(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public string GetRole(ApplicationUser user)
        {
            var roles = this.roleManager.Roles.ToList();

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
