using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Models.Entities
{
    public class User
    {
        public long Id { get; set; }

        public ApplicationUser AccountUser { get; set; }
       // public long AccountUserId { get; set; }

        public DateTime DateRegister { get; set; }
        public string Email { get; set; }
    }
}
