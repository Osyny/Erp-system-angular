using Erp_ang2.Data;
using Erp_ang2.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Controllers.ProjectTypes
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectTypesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ProjectTypesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        // GET: api/projectTypes  
        [HttpGet]
        public async Task<ActionResult<List<ProjectType>>> Get()
        {
            var entity = await this.dbContext.Types
                 .ToListAsync();
            return Ok(entity);
        }
    }
}
