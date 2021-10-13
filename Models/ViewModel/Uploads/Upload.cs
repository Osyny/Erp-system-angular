using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Models.ViewModel.Uploads
{
    public class Upload
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Directory { get; set; }
        public string Path => $"{Directory}{FileName}";
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }
    }
}
