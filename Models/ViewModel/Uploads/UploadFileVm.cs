using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Models.ViewModel.Uploads
{
    public class UploadFileVm
    {
        public long ProjectId { get; set; }
        public IFormFile UploadFile { get; set; }
        public string File { get; set; }
       
    }
}
