using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Models.Entities
{
    public class Skill
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public Project Project { get; set; }
        public long ProjectId { get; set; }
    }
}
