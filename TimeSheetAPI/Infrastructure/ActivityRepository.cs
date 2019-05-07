using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Infrastructure
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly TimeSheetContext TimeSheetContext;
        public ActivityRepository(TimeSheetContext TimeSheetContext)
        {
            this.TimeSheetContext = TimeSheetContext;
        }
        public Task<Activity> GetActivity(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
