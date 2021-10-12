using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Models.ViewModel.Projects
{
    public class EditProjectVm
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public string Organization { get; set; }    

       // public string Role { get; set; }
        public string Link { get; set; }

        public string User { get; set; }

        public string SelectedTypeId { get; set; }
    }
}
