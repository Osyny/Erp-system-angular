using Erp_ang2.Data;
using Erp_ang2.Helpers;
using Erp_ang2.Models;
using Erp_ang2.Models.Entities;
using Erp_ang2.Models.ViewModel.Projects;
using Erp_ang2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Erp_ang2.Controllers.Projects
{
    [ApiController]
    [Route("[controller]")]
    public class ProgectsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;

        public ProgectsController(ICurrentUserService currentUserService,
             RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
        }


        // GET: /progects  
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
                  Attachments = pr.Attachments.Count != 0 ? string.Join(", ", pr.Attachments.Select(s => s.FileName).ToArray()) : "",
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

            var roles = this.roleManager.Roles.ToList();
            if (roles.Count == 0 && !roles.Any())
            {
                IdentityResult resultAdmin = await this.roleManager.CreateAsync(new IdentityRole("Admin"));
                IdentityResult resultUser = await this.roleManager.CreateAsync(new IdentityRole("User"));

                var admin = await dbContext.ListUsers.Include(u => u.AccountUser)
              .FirstOrDefaultAsync(u => u.AccountUser.Email == "admin@gmail.com");

                if (admin == null)
                {
                    ApplicationUser accountUser = new ApplicationUser { Email = "admin@gmail.com", UserName = "admin@gmail.com" };
                    var res = await this.userManager.CreateAsync(accountUser, "Q1234_qaz");
                    await this.userManager.AddToRoleAsync(accountUser, "Admin");

                    string emailConfirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(accountUser);
                    var confirmResult = await this.userManager.ConfirmEmailAsync(accountUser, emailConfirmationToken);

                    if (res.Succeeded)
                    {
                        var newUser = new User()
                        {
                            AccountUser = accountUser,
                            DateRegister = DateTime.Now

                        };
                        dbContext.ListUsers.Add(newUser);
                        await dbContext.SaveChangesAsync();         
                    }
                }
            }
            if (allType.Count == 0)
            {
                var createDefaultData = new DefaultDataTypes(this.dbContext);
                mess = createDefaultData.GenerateDefaultData();
            }

            //var us = User.Identity.Name;
            //var userName = HttpContext.User.Identity.Name;

            var u = _currentUserService;

            if (progects.Any())
                prVm.ProjectsVm = progects;

            return Ok(prVm);
        }

        // GET: /Progects/5  
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

        // GET: /Progects/about/5  
        [HttpGet("about/{projectId}")]
        public async Task<ActionResult<AboutProjectVm>> AboutProject(long projectId)
        {

            var prAll = await dbContext.Projects
                .Include(p => p.Attachments)
                .Include(p => p.ProjectType)
                .Include(p => p.Skills)
                .ToListAsync();
            var updatePr = prAll.FirstOrDefault(p => p.Id == projectId);

            var allTypes = this.dbContext.Types.ToList();

            var model = new AboutProjectVm()
            {

                Id = updatePr.Id,
                Title = updatePr.Title,
                Description = updatePr.Description,
                Organization = updatePr.Organization,
                End = updatePr.End != null || updatePr.End.HasValue ? updatePr.End.Value.ToString("dd.MM.yyyy hh:mm") : "",

                Start = updatePr.Start != null || updatePr.Start.HasValue ? updatePr.Start.Value.ToString("dd.MM.yyyy") : "",
                Role = updatePr.Role,

                AttachmentVm = updatePr.Attachments.Select(f => new AboutFileVm()
                {
                    Id = f.Id,
                    File = f.File,
                    FileName = f.FileName,
                    Date = f.DateCreate.ToString("dd.MM.yyyy")
                }).ToList(),
                SkillsVm = updatePr.Skills == null ? new List<AboutSkillVm>() : updatePr.Skills.Select(s => new AboutSkillVm()
                {
                    Id = s.Id,
                    SkillName = s.Name,

                }).ToList(),

                ProjectType = updatePr.ProjectType.NameType,
                Create = updatePr.Created.ToString("dd.MM.yyyy"),
                Update = updatePr.Updated.ToString("dd.MM.yyyy"),
            };
            return model;
        }

        // POST: /Progects  
        [HttpPost]
        public async Task<ActionResult<long>> Post([FromForm] CreateProjectVm request)
        {
            var newProject = new Project();

            var rolesUser = new Roles(this.roleManager, this.userManager);

            var allUsers = dbContext.ListUsers.Include(u => u.AccountUser).ToListAsync();
            string userRole = "No_Authorize";
            if (!string.IsNullOrEmpty(request.UserName))
            {
                //var curentuser = await dbContext.ListUsers
                //    .Include(u => u.AccountUser)
                //    .FirstOrDefaultAsync(u => u.AccountUser.Email == request.UserName);
                var curentuser = await this.userManager.FindByEmailAsync(request.UserName);

                if (curentuser != null)
                {
                    userRole = rolesUser.GetRole(curentuser);
                }
            }

            long typeId = 0;
            if (long.TryParse(request.SelectedTypeId, out typeId)
                && !string.IsNullOrEmpty(request.Title) && !string.IsNullOrEmpty(request.Description))
            {
                var type = this.dbContext.Types.FirstOrDefaultAsync(t => t.Id == typeId);

                newProject.Title = request.Title;

                newProject.Description = request.Description;
                newProject.Organization = request.Organization;
                newProject.Role = userRole;
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


        // PUT: /Progects/5  
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> EditProject(long id, [FromForm] EditProjectVm model)
        {
            var updatePr = await dbContext.Projects
                .Include(p => p.Attachments)
                .FirstOrDefaultAsync(p => p.Id == id);
            var res = false;
            if (updatePr != null)
            {
                if (!string.IsNullOrEmpty(model.Title))
                {
                    updatePr.Title = model.Title;
                }

                if (!string.IsNullOrEmpty(model.Description))
                {
                    updatePr.Description = model.Description;
                }

                if (!string.IsNullOrEmpty(model.Organization))
                {
                    updatePr.Organization = model.Organization;
                }

                long typeId = 0;
                if (long.TryParse(model.SelectedTypeId, out typeId))
                {
                    updatePr.ProjectTypeId = typeId;
                }
                updatePr.Updated = DateTime.Now;

                dbContext.Update(updatePr);
                await dbContext.SaveChangesAsync();
                res = true;
            }
            return res;
        }


        // DELETE: /Progects/5  
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(long id)
        {
            var pr = await dbContext.Projects
           .FirstOrDefaultAsync(p => p.Id == id);
            var res = false;
            if (pr != null)
            {
                res = true;
                dbContext.Projects.Remove(pr);

                dbContext.SaveChanges();
            }
            return res;
        }
    }
}
