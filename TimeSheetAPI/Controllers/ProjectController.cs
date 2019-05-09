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

            var ModelProject = new Models.Project { Name = Project.Name};

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
        public async Task<ActionResult> Delete(Dto.ProjectForDelete Project)
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
        public async Task<ICollection<Dto.ProjectWithoutLogs>> GetList()
        {
            var Project = await TimeSheetContext.Project.ToListAsync();
            List<Dto.ProjectWithoutLogs> projects = new List<ProjectWithoutLogs>();
            foreach (var item in Project)
            {
                projects.Add(new ProjectWithoutLogs { Id = item.Id, Name = item.Name, CompanyId = item.CompanyId, Billable = item.Billable, Overtime = item.Overtime });
            }
            return projects;
        }
        [AllowAnonymous]
        [HttpGet("Test")]
        public async Task<List<Dto.Project>> Test()
        {
            List<Models.Project> projects = await TimeSheetContext.Project.Include(x => x.Activitys).Include(x => x.UsersOnTheProject).Include(x => x.Logs).ToListAsync();
            List<Dto.Project> projectsDto = new List<Project>();
            foreach (var project in projects)
            {
                List<string> activityIds = new List<string>();
                foreach (var activity in project.Activitys)
                {
                    activityIds.Add(activity.Id);
                }
                List<string> logsIds = new List<string>();
                foreach (var log in project.Logs)
                {
                    logsIds.Add(log.Id);
                }
                List<string> userIds = new List<string>();
                foreach (var user in project.UsersOnTheProject)
                {
                    userIds.Add(user.Id);
                }
                projectsDto.Add(new Project { Id = project.Id, Name = project.Name, CompanyId = project.CompanyId, Billable = project.Billable, Overtime = project.Overtime, ActivitysId = activityIds, LogsId = logsIds, UsersId = userIds });
            }
            return projectsDto;
        }
    }
}