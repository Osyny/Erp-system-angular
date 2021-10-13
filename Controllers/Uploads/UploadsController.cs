using Erp_ang2.Data;
using Erp_ang2.Helpers;
using Erp_ang2.Models.Entities;
using Erp_ang2.Models.ViewModel.Uploads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Controllers.Uploads
{
    [ApiController]
    [Route("[controller]")]
    public class UploadsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHostingEnvironment environment;

        public UploadsController(ApplicationDbContext dbContext,
            IHostingEnvironment environment)
        {
            this.dbContext = dbContext;
            this.environment = environment;
        }


        // POST: posts
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> Create([FromForm] UploadFileVm model)
        {
            var updatePr = await dbContext.Projects
              .Include(p => p.Attachments)
              .FirstOrDefaultAsync(p => p.Id == model.ProjectId);

            var result = false;
            if (Request.Form.Files.Any())
            {
                foreach (var file in Request.Form.Files)
                {
                    string name = file.FileName;
                    string path = $"/files/{name}";
                    string serverPath = $"{this.environment.WebRootPath}{path}";
                    FileStream fs = new FileStream(serverPath, FileMode.Create,
                        FileAccess.Write);
                    await file.CopyToAsync(fs);
                    fs.Close();

                    var newFile = new DbFile()
                    {
                        File = path,
                        FileName = name,
                        DateCreate = DateTime.Now
                    };

                    dbContext.DbFiles.Add(newFile);
                    dbContext.SaveChanges();


                    updatePr.Attachments.Add(newFile);
                    updatePr.Updated = DateTime.Now;
                    dbContext.Update(updatePr);

                    await dbContext.SaveChangesAsync();
                    result = true;
                }
            }
            return result;
        }
    }
}
