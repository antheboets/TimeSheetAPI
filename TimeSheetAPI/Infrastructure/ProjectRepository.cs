using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetAPI.Models;
using Microsoft.Extensions.Configuration;

namespace TimeSheetAPI.Infrastructure
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TimeSheetContext TimeSheetContext;
        private readonly IConfiguration Config;
        public ProjectRepository(TimeSheetContext TimeSheetContext, IConfiguration Config)
        {
            this.TimeSheetContext = TimeSheetContext;
            this.Config = Config;
        }
        public async Task<bool> Create(Models.Project project)
        {
            if (project == null)
            {
                return false;
            }
            if (project.CompanyId == null) {
                return false;
            }
            if (project.CompanyId == "")
            {
                return false;
            }
            project.Company = new Models.Company { Id= project.Id };
            await TimeSheetContext.AddAsync(project);
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }
        public async Task<Project> GetSmall(Models.Project project)
        {
            if (project == null)
            {
                return null;
            }
            if (project.Id == null)
            {
                return null;
            }
            if (project.Id == "")
            {
                return null;
            }
            return await TimeSheetContext.Project.Where(x => x.Id == project.Id).SingleOrDefaultAsync();
        }
        public async Task<Project> GetFull(Models.Project project)
        {
            if (project == null)
            {
                return null;
            }
            if (project.Id == null)
            {
                return null;
            }
            if (project.Id == "")
            {
                return null;
            }
            project = await TimeSheetContext.Project.Where(x => x.Id == project.Id).Include(x => x.Users).Include(x => x.Logs).Include(x => x.Activitys).Include(x => x.Company).SingleOrDefaultAsync();
            foreach (Models.ProjectUser projectUser in project.Users)
            {
                projectUser.User = await TimeSheetContext.User.Where(x => x.Id == projectUser.UserId).SingleOrDefaultAsync();
            }
            return project;
        }

        public async Task<bool> Delete(Models.Project project)
        {
            project = await GetSmall(project);
            if (project == null)
            {
                return false;
            }
            try
            {
                project = await TimeSheetContext.Project.Where(x => x.Id == project.Id).Include(x => x.Users).Include(x => x.Activitys).Include(x => x.Logs).SingleOrDefaultAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            bool hasUsers = false;
            if (project.Users == null)
            {
                return false;
            }
            if (project.Users.Count > 0)
            {
                hasUsers = true;
            }
            if (hasUsers)
            {
                foreach (Models.ProjectUser user in project.Users)
                {
                    TimeSheetContext.Remove(user);
                }
            }
            bool hasActivity = false;
            if (project.Activitys == null)
            {
                return false;
            }
            if (project.Activitys.Count > 0)
            {
                hasActivity = true;
            }
            if (hasActivity)
            {
                foreach (Models.Activity activity in project.Activitys)
                {
                    TimeSheetContext.Remove(activity);
                }
            }
            bool hasLog = false;
            if (project.Logs == null)
            {
                return false;
            }
            if (project.Logs.Count > 0)
            {
                hasLog = true;
            }
            if (hasLog)
            {
                foreach (Models.Log log in project.Logs)
                {
                    TimeSheetContext.Remove(log);
                }
            }
            TimeSheetContext.Remove(project);
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Update(Models.Project project)
        {
            if (project == null)
            {
                return false;
            }
            if (project.Id == null)
            {
                return false;
            }
            if (project.Id == "")
            {
                return false;
            }
            TimeSheetContext.Update(project);
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<Models.Project>> GetAll()
        {
            return await TimeSheetContext.Project.ToListAsync();
        }
        public async Task<List<Models.Project>> GetAllOfUser(string userId)
        {
            if (userId == null)
            {
                return null;
            }
            if (userId == "")
            {
                return null;
            }
            List<Models.Project> list = await TimeSheetContext.Project.Include(x => x.Users).Where(x => x.InProgress == true).ToListAsync();
            //TODO Replace with linq
            List<Models.Project> projectsList = new List<Models.Project>();
            //bool containsUser;
            foreach (Models.Project project in list)
            {
                /*
                containsUser = false;
                foreach (Models.User user in project.UsersOnTheProject)
                {
                    if (user.Id == userId)
                    {
                        containsUser = true;
                    }
                }
                if (containsUser)
                {
                    projectsList.Add(project);
                }
                */
                projectsList.Add(project);
            }
            return projectsList;
        }
        public async Task<bool> AddUserToProject(Project project, List<User> users)
        {
            if (project == null)
            {
                return false;
            }
            if (users == null)
            {
                return false;
            }
            if (project.Id == null)
            {
                return false;
            }
            if (project.Id == "")
            {
                return false;
            }
            try
            {
                project = await TimeSheetContext.Project.Where(x => x.Id == project.Id).SingleOrDefaultAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            if (project == null)
            {
                return false;
            }
            foreach (Models.User user in users)
            {
                if (user == null)
                {
                    return false;
                }
                if (user.Id == null)
                {
                    return false;
                }
                if (user.Id == "")
                {
                    return false;
                }
                try
                {
                    Models.User userFromDb = await TimeSheetContext.User.Where(x => x.Id == user.Id).SingleOrDefaultAsync();
                    if (userFromDb == null)
                    {
                        return false;
                    }
                    Models.ProjectUser projectUser = new ProjectUser { UserId = userFromDb.Id, ProjectId = project.Id };
                    await TimeSheetContext.ProjectUser.AddAsync(projectUser);
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveUsers(Models.Project project, List<Models.User> users)
        {
            if (project == null)
            {
                return false;
            }
            if (project.Id == null)
            {
                return false;
            }
            if (project.Id == "")
            {
                return false;
            }
            if (users == null)
            {
                return false;
            }
            if (users.Count <= 0)
            {
                return false;
            }
            foreach (Models.User user in users)
            {
                if (user == null)
                {
                    return false;
                }
                if (user.Id == null)
                {
                    return false;
                }
                if (user.Id == "")
                {
                    return false;
                }
                try
                {
                    Models.ProjectUser UserOnProject = await TimeSheetContext.ProjectUser.Where(x => x.UserId == user.Id && x.ProjectId == project.Id).SingleOrDefaultAsync();
                    TimeSheetContext.Remove(UserOnProject);
                }
                catch(Exception e)
                {
                    return false;
                }
            }
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddUsers(Models.Project project, List<Models.User> users)
        {
            if (project == null)
            {
                return false;
            }
            if (project.Id == null)
            {
                return false;
            }
            if (project.Id == "")
            {
                return false;
            }
            if (users == null)
            {
                return false;
            }
            if (users.Count <= 0)
            {
                return false;
            }
            foreach (Models.User user in users)
            {
                if (user == null)
                {
                    return false;
                }
                if (user.Id == null)
                {
                    return false;
                }
                if (user.Id == "")
                {
                    return false;
                }
                try
                {
                    await TimeSheetContext.User.Where(x => x.Id == user.Id && x.RoleId == Config.GetSection("Role:Consultant:Id").Value).SingleAsync();
                    TimeSheetContext.ProjectUser.Add(new Models.ProjectUser { UserId = user.Id, ProjectId = project.Id});
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }
    }
}
