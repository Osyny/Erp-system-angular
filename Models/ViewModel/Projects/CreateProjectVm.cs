using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Models.ViewModel.Projects
{
    public class CreateProjectVm
    {
        public string Title { get; set; }

        public string Description { get; set; }
        public string Organization { get; set; }

        public string SelectedTypeId { get; set; }
    }
}
