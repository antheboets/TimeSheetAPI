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
using Microsoft.Extensions.Configuration;
using System.Text;
using AutoMapper;

namespace TimeSheetAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository Repo;
        private readonly IConfiguration Config;
        private readonly IMapper Mapper;

        public ProjectController(IProjectRepository Repo, IConfiguration Config, IMapper Mapper)
        {
            this.Repo = Repo;
            this.Config = Config;
            this.Mapper = Mapper;
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody]Dto.ProjectForCreate Project)
        {
            if (User.FindFirst(ClaimTypes.Role).Value == Config.GetSection("Role:Consultant:Name").Value)
            {
                return Unauthorized();
            }
            bool hasUsers = false;
            if (Project == null)
            {
                return BadRequest();
            }
            if (Project.UsersOnTheProject == null)
            {
                return BadRequest();
            }
            if (Project.UsersOnTheProject.Count > 0)
            {
                hasUsers = true;
            }
            List<Models.User> users = new List<Models.User>();
            foreach (Dto.UserId user in Project.UsersOnTheProject)
            {
                users.Add(new Models.User { Id = user.Id });
            }
            Models.Project ModelProject = new Models.Project { Name = Project.Name, CompanyId = Project.CompanyId, InProgress = Project.InProgress, Billable = Project.Billable, Overtime = Project.Overtime };
            if (await Repo.Create(ModelProject))
            {
                if (hasUsers)
                {
                    if (await Repo.AddUserToProject(ModelProject, users))
                    {
                        return StatusCode(201);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return StatusCode(201);
                }
            }
            return BadRequest();
        }
        [HttpPost("GetFull")]
        public async Task<ActionResult<Dto.ProjectForGetFull>> GetFull([FromBody]Dto.ProjectForGet Project)
        {
            if (Project == null)
            {
                return BadRequest();
            }
            if (Project.Id == "")
            {
                return BadRequest();
            }
            Models.Project projectModel = await Repo.GetFull(new Models.Project { Id = Project.Id });
            if (projectModel == null)
            {
                return BadRequest();
            }
            List<Dto.Log> logs = new List<Dto.Log>();
            foreach (Models.Log log in projectModel.Logs)
            {
                logs.Add(new Log { Id = log.Id, Start = log.Start, Stop = log.Stop, ActivityId = log.ActivityId, Description = log.Description, UserId = log.UserId });
            }
            List<Dto.Activity> activities = new List<Activity>();
            foreach (Models.Activity activity in projectModel.Activitys)
            {
                activities.Add(new Activity { Id = activity.Id, Name = activity.Name });
            }
            List<Dto.UserForGet> users = new List<UserForGet>();
            foreach (Models.ProjectUser projectUser in projectModel.Users)
            {
                users.Add(new Dto.UserForGet { Id = projectUser.User.Id, Name = projectUser.User.Name, RoleId = projectUser.User.RoleId, Email = projectUser.User.Email,ChangeHistory= projectUser.User.ChangeHistory });
            }
            return Ok(new Dto.ProjectForGetFull { Id = projectModel.Id, Logs = logs, Activitys = activities, UsersOnTheProject = users, InProgress = projectModel.InProgress, Billable = projectModel.Billable, Overtime = projectModel.Overtime, Name = projectModel.Name, Company = new Company { Id = projectModel.Company.Id, Name = projectModel.Company.Name } });
        }
        [HttpPost("GetSmall")]
        public async Task<ActionResult<Dto.ProjectForGetSmall>> GetSmall([FromBody]Dto.ProjectForGet Project)
        {
            if (Project == null)
            {
                return BadRequest();
            }
            if (Project.Id == "")
            {
                return BadRequest();
            }
            Models.Project projectModel = await Repo.GetFull(new Models.Project { Id = Project.Id });
            if (projectModel == null)
            {
                return BadRequest();
            }
            List<string> logIds = new List<string>();
            foreach (Models.Log log in projectModel.Logs)
            {
                logIds.Add(log.Id);
            }
            List<string> activitieIds = new List<string>();
            foreach (Models.Activity activity in projectModel.Activitys)
            {
                activitieIds.Add(activity.Id);
            }
            List<string> userIds = new List<string>();
            foreach (Models.ProjectUser user in projectModel.Users)
            {
                userIds.Add(user.UserId);
            }
            return Ok(new Dto.ProjectForGetSmall { Id = projectModel.Id, LogsId = logIds, ActivitysId = activitieIds, UsersId = userIds, InProgress = projectModel.InProgress, Billable = projectModel.Billable, Overtime = projectModel.Overtime, Name = projectModel.Name, CompanyId = projectModel.Company.Id });
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Update([FromBody]Dto.ProjectForUpdate project)
        {
            if (User.FindFirst(ClaimTypes.Role).Value == Config.GetSection("Role:Consultant:Name").Value)
            {
                return Unauthorized();
            }
            List<Models.Activity> activities = new List<Models.Activity>();
            foreach (Dto.Activity activity in project.Activitys)
            {
                activities.Add(new Models.Activity { Id = activity.Id, Name = activity.Name, ProjectId = activity.ProjectId });
            }
            List<Models.User> userIds = new List<Models.User>();
            foreach (string user in project.UserId)
            {
                userIds.Add(new Models.User { Id = user });
            }
            Models.Project ModelProject = new Models.Project { Id = project.Id, Name = project.Name, CompanyId = project.CompanyId, Activitys = activities, Billable = project.Billable, Overtime = project.Overtime, InProgress = project.InProgress };
            if (await Repo.Update(ModelProject))
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([FromBody]Dto.ProjectForDelete project)
        {
            if (User.FindFirst(ClaimTypes.Role).Value == Config.GetSection("Role:Consultant:Name").Value)
            {
                return Unauthorized();
            }
            Models.Project ModelProject = new Models.Project { Id = project.Id };
            if (await Repo.Delete(ModelProject))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("GetList")]
        public async Task<ActionResult<ICollection<Dto.ProjectForGetSmall>>> GetList()
        {
            List<Models.Project> projectModel = await Repo.GetAllOfUser(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (projectModel.Count == 0)
            {
                return BadRequest();
            }
            List<Dto.ProjectForGetSmall> projectsDto = new List<ProjectForGetSmall>();
            foreach (var project in projectModel)
            {
                List<string> activityIds = new List<string>();
                List<string> logsIds = new List<string>();
                List<string> userIds = new List<string>();

                if (project.Activitys != null)
                {
                    foreach (var activity in project.Activitys)
                    {
                        activityIds.Add(activity.Id);
                    }
                }

                if (project.Logs != null)
                {
                    foreach (var log in project.Logs)
                    {
                        logsIds.Add(log.Id);
                    }
                }

                if (project.Users != null)
                {
                    foreach (var user in project.Users)
                    {
                        userIds.Add(user.UserId);
                    }
                }
                Dto.ProjectForGetSmall projectForGetSmall = new Dto.ProjectForGetSmall { Id = project.Id, Billable = project.Billable, InProgress = project.InProgress, Name = project.Name, Overtime = project.Overtime, CompanyId = project.CompanyId };
                //Mapper.Map<Dto.ProjectForGetSmall>(project);
                projectForGetSmall.UsersId = userIds;
                projectForGetSmall.ActivitysId = activityIds;
                projectForGetSmall.LogsId = logsIds;
                projectsDto.Add(projectForGetSmall);
            }
            //return Mapper.Map<List<Dto.ProjectForGetSmall>>(projects);
            return Ok(projectsDto);
        }
        [HttpPost("RemoveUser")]
        public async Task<ActionResult> RemoveUser(Dto.ProjectForUserList projectForUserList)
        {
            if (User.FindFirst(ClaimTypes.Role).Value == Config.GetSection("Role:Consultant:Name").Value)
            {
                return Unauthorized();
            }
            if (projectForUserList == null)
            {
                return BadRequest();
            }
            if (projectForUserList.UserIds == null)
            {
                return BadRequest();
            }
            List<Models.User> userList = new List<Models.User>();
            foreach (Dto.UserId user in projectForUserList.UserIds)
            {
                userList.Add(new Models.User { Id = user.Id });
            }
            Models.Project project = new Models.Project { Id = projectForUserList.ProjectId };
            if (await Repo.RemoveUsers(project, userList))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("AddUser")]
        public async Task<ActionResult> AddUser(Dto.ProjectForUserList projectForUserList)
        {
            if (User.FindFirst(ClaimTypes.Role).Value == Config.GetSection("Role:Consultant:Name").Value)
            {
                return Unauthorized();
            }
            if (projectForUserList == null)
            {
                return BadRequest();
            }
            if (projectForUserList.UserIds == null)
            {
                return BadRequest();
            }
            List<Models.User> userList = new List<Models.User>();
            foreach (Dto.UserId user in projectForUserList.UserIds)
            {
                userList.Add(new Models.User {Id = user.Id});
            }
            Models.Project project = new Models.Project { Id = projectForUserList.ProjectId};
            if (await Repo.AddUsers(project, userList))
            {
                return Ok();
            }
            return BadRequest();
        }
        [AllowAnonymous]
        [HttpGet("Test")]
        public async Task<ActionResult<List<Dto.ProjectForGetSmall>>> Test()
        {
            List<Models.Project> projects = await Repo.GetAll();
            List<Dto.ProjectForGetSmall> projectsDto = new List<ProjectForGetSmall>();

            foreach (var project in projects)
            {
                List<string> activityIds = new List<string>();
                List<string> logsIds = new List<string>();
                List<string> userIds = new List<string>();

                if (project.Activitys != null)
                {
                    foreach (var activity in project.Activitys)
                    {
                        activityIds.Add(activity.Id);
                    }
                }

                if (project.Logs != null)
                {
                    foreach (var log in project.Logs)
                    {
                        logsIds.Add(log.Id);
                    }
                }

                if (project.Users != null)
                {
                    foreach (var user in project.Users)
                    {
                        userIds.Add(user.UserId);
                    }
                }
                Dto.ProjectForGetSmall projectForGetSmall = new Dto.ProjectForGetSmall { Id = project.Id, Billable = project.Billable, InProgress = project.InProgress, Name = project.Name, Overtime = project.Overtime, CompanyId = project.CompanyId };
                //Mapper.Map<Dto.ProjectForGetSmall>(project);
                projectForGetSmall.UsersId = userIds;
                projectForGetSmall.ActivitysId = activityIds;
                projectForGetSmall.LogsId = logsIds;
                projectsDto.Add(projectForGetSmall);
            }

            //return Mapper.Map<List<Dto.ProjectForGetSmall>>(projects);
            return Ok(projectsDto);
        }
    }
}