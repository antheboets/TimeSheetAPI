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
        public async Task<bool> Create(Activity activity)
        {
            if (activity == null)
            {
                return false;
            }
            if (activity.Id == null)
            {
                return false;
            }
            if (activity.Id == "")
            {
                return false;
            }
            if (activity.ProjectId == null)
            {
                return false;
            }
            if (activity.ProjectId == "")
            {
                return false;
            }
            await TimeSheetContext.Activity.AddAsync(activity);
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }

        public Task<Activity> Get(Activity activity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IActivityRepository.Delete(Activity activity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IActivityRepository.Update(Activity activity)
        {
            throw new NotImplementedException();
        }
    }
}
