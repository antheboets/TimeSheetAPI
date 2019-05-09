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

        public Task<Project> Get(Project project)
        {
            throw new NotImplementedException();
        }
    }
}
