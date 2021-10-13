using Erp_ang2.Models.ViewModel.Uploads;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Helpers
{
    public static class UploadExtensions
    {
        public static async Task<Upload> CreateUpload(this IFormFile file)
        {
            var fileBytes = new byte[file.Length];
            await file.OpenReadStream().ReadAsync(fileBytes, 0, int.Parse(file.Length.ToString()));

            return new Upload
            {
                Name = file.FileName,
                FileName = file.FileName,
                Bytes = fileBytes,
                ContentType = file.ContentType
            };
        }
    }
}
