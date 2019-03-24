using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TimeSheetAPI.Dto;
using TimeSheetAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TimeSheetAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly TimeSheetContext TimeSheetContext;
        public ProjectController(TimeSheetContext timeSheetContext)
        {
            this.TimeSheetContext = timeSheetContext;
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create(Dto.ProjectForCreate Project)
        {
            if (User.FindFirst(ClaimTypes.Role).Value == "")
            {
                Unauthorized();
            }
            var ModelProject = new Models.Project { Name = Project.Name, Company = new Models.Company { Id = Project.CompanyId } };
            await TimeSheetContext.AddAsync(ModelProject);
            await TimeSheetContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Upadate(Dto.ProjectForUpdate Project)
        {
            if (User.FindFirst(ClaimTypes.Role).Value == "")
            {
                Unauthorized();
            }
            var ModelProject = new Models.Project { Id= Project.Id, Name = Project.Name, Company = new Models.Company { Id = Project.CompanyId }, Activitys = Project.Activitys };
            TimeSheetContext.Update(ModelProject);
            await TimeSheetContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete(Dto.ProjectForUpdate Project)
        {
            if (User.FindFirst(ClaimTypes.Role).Value == "")
            {
                Unauthorized();
            }
            Models.Project ModelProject = await TimeSheetContext.Project.Where(x => x.Id == Project.Id).SingleOrDefaultAsync();
            if (ModelProject == null)
            {
                BadRequest();
            }
            TimeSheetContext.Remove(ModelProject);
            await TimeSheetContext.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("GetList")]
        public async Task<ICollection<ProjectWithoutLogs>> GetList()
        {
           
            return null;
        }
    }
}
