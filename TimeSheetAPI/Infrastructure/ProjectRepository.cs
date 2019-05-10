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

        public async Task<bool> Create(Project project)
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

        public async Task<Project> GetSmall(Project project)
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
        public async Task<Project> GetFull(Project project)
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

        public async Task<bool> Delete(Project project)
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
        public async Task<bool> Update(Project project)
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

        public Task<List<Project>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
