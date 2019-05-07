using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Infrastructure
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TimeSheetContext TimeSheetContext;
        public ProjectRepository(TimeSheetContext TimeSheetContext)
        {
            this.TimeSheetContext = TimeSheetContext;
        }

    }
}
