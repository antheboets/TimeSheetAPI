using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Infrastructure
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TimeSheetContext TimeSheetContext;
        public ProjectRepository(TimeSheetContext TimeSheetContext)
        {
            this.TimeSheetContext = TimeSheetContext;
        }

        public async Task<bool> Create(Models.Project project)
        {
            if (project == null)
            {
                return false;
            }
            if (project.Company == null) {
                return false;
            }
            if (project.Company == null)
            {
                return false;
            }
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
            return await TimeSheetContext.Project.Where(x => x.Id == project.Id).Include(x => x.UsersOnTheProject).ThenInclude(x => x.Role).Include(x => x.Logs).Include(x => x.Activitys).Include(x => x.Company).SingleOrDefaultAsync();
        }

        public async Task<bool> Delete(Models.Project project)
        {
            project = await GetSmall(project);
            if (project == null)
            {
                return false;
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
            List<Models.Project> list = await TimeSheetContext.Project.Include(x => x.UsersOnTheProject).Where(x => x.InProgress == true).ToListAsync();
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
    }
}
