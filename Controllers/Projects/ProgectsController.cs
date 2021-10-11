using Erp_ang2.Data;
using Erp_ang2.Helpers;
using Erp_ang2.Models.Entities;
using Erp_ang2.Models.ViewModel.Projects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Controllers.Projects
{
    [ApiController]
    [Route("[controller]")]
    public class ProgectsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        //private readonly RoleManager<IdentityRole> roleManager;
        //private readonly UserManager<AccountUser> userManager;

        public ProgectsController(
            // RoleManager<IdentityRole> roleManager,
            //UserManager<AccountUser> userManager,
            ApplicationDbContext dbContext)
        {
            //  this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            //this.roleManager = roleManager;
            //this.userManager = userManager;
            this.dbContext = dbContext;
        }


        // GET: api/progects  
        [HttpGet]
        public async Task<ActionResult<GetProjectListVm>> Get()
        {
            var progects = await this.dbContext.Projects
                .Include(p => p.Skills)
                 .Include(p => p.ProjectType)
                  .Include(p => p.Attachments)
              .Select(pr => new ProjectViewModel()
              {
                  Id = pr.Id,
                  Title = pr.Title,
                  Description = pr.Description,
                  Organization = pr.Organization,
                  End = pr.Start.HasValue ? pr.Start.Value.ToString("dd.MM.yyyy") : "--",

                  Start = pr.Start.HasValue ? pr.Start.Value.ToString("dd.MM.yyyy") : "--",
                  Role = pr.Role,
                  Link = string.IsNullOrEmpty(pr.Link) ? "" : pr.Link,
                  Skills = pr.Skills.Count != 0 ? string.Join(", ", pr.Skills.Select(s => s.Name).ToArray()) : "",
                  Attachments = pr.Attachments.Count != 0 ? string.Join(", ", pr.Attachments.Select(s => s.File).ToArray()) : "",
                  ProjectType = pr.ProjectType.NameType,
                  Create = pr.Created.ToString("dd.MM.yyyy"),
                  Update = pr.Updated.ToString("dd.MM.yyyy")
              }).ToListAsync();

            var prVm = new GetProjectListVm()
            {
                ProjectsVm = new List<ProjectViewModel>()
            };

            var mess = "";
            var allType = dbContext.Types.ToList();

            if (allType.Count == 0)
            {
                var createDefaultData = new DefaultDataTypes(this.dbContext);
                mess = createDefaultData.GenerateDefaultData();
            }

            if (progects.Any())
                prVm.ProjectsVm = progects;

            //var roles = this.roleManager.Roles.ToList();
            //var allUsers = dbContext.ListUsers.Include(u => u.AccountUser).ToList();
            //var mess = "";
            //if (roles.Count == 0 && !roles.Any() && !allUsers.Any())
            //{
            //    var createToDb = new CreateStartData(roleManager, dbContext, userManager);
            //    var res = createToDb.StartCreate();
            //    mess = res.Result;
            //}
            //prVm.Message = mess;
            return Ok(prVm);
        }

        // GET: api/Progects/5  
        [HttpGet("{projectId}", Name = "Get")]
        public async Task<Project> GetProgectById(long projectId)
        {
            var progect = await this.dbContext.Projects
               .Include(p => p.Skills)
                .Include(p => p.ProjectType)
                 .Include(p => p.Attachments)
             .FirstOrDefaultAsync(pr => pr.Id == projectId);
            return progect;
        }

        // POST: api/Progects  
        [HttpPost]
        public async Task<ActionResult<long>> Post([FromForm] CreateProjectVm request)
        {
            var newProject = new Project();
            long typeId = 0;
            if (long.TryParse(request.SelectedType, out typeId)
                && !string.IsNullOrEmpty(request.Title) && !string.IsNullOrEmpty(request.Description))
            {
                var type = this.dbContext.Types.FirstOrDefaultAsync(t => t.Id == typeId);

                newProject.Title = request.Title;

                newProject.Description = request.Description;
                newProject.Organization = request.Organization;
                newProject.Role = "User";
                // newProduct.Link = request.Link;

                newProject.ProjectTypeId = typeId;

                newProject.Created = DateTime.Now;
                newProject.Updated = DateTime.Now;

                dbContext.Projects.Add(newProject);
                await dbContext.SaveChangesAsync();


                return newProject.Id;
            }
            else
            {
                return 0;
            }
        }


        public string EditProject([FromForm] EditProjectVm model)
        {
            var updatePr = dbContext.Projects
                .Include(p => p.Attachments)
                .FirstOrDefault(p => p.Id == model.Id);

            updatePr.Title = model.Title;
            updatePr.Description = model.Description;
            updatePr.Organization = model.Organization;
            updatePr.Role = model.Role;
            updatePr.Link = model.Link;

            updatePr.ProjectTypeId = type.Id;
            updatePr.Updated = DateTime.Now;

            dbContext.Update(updatePr);
            dbContext.SaveChanges();
            mes = "Project змінено";

            return mes;
        }

        // DELETE: api/ApiWithActions/5  
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
