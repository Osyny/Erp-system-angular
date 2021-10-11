using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Models.Entities
{
    public class DbFile
    {
        public long Id { get; set; }
        public string File { get; set; }
        public string FileName { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
