using Microsoft.EntityFrameworkCore;
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
        public async Task<Models.Activity> Get(Models.Activity activity)
        {
            if (activity == null)
            {
                return null;
            }
            if (activity.Id == null)
            {
                return null;
            }
            if (activity.Id == "")
            {
                return null;
            }
            Models.Activity activityGet = null;
            try
            {
                activityGet = await TimeSheetContext.Activity.Where(x => x.Id == activity.Id).SingleOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
            if (activityGet == null)
            {
                return null;
            }
            return activityGet;
        }
        public async Task<List<Models.Activity>> GetAll()
        {
            return await TimeSheetContext.Activity.ToListAsync();
        }
        public async Task<bool> Delete(Models.Activity activity)
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
            try
            {
                Models.Activity activityGet = await TimeSheetContext.Activity.Where(x => x.Id == activity.Id).SingleOrDefaultAsync();
                if (activityGet == null)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            TimeSheetContext.Remove(activity);
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Update(Models.Activity activity)
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
            try{
                Models.Activity activityOld = await TimeSheetContext.Activity.Where(x => x.Id == activity.Id).SingleOrDefaultAsync();
                activity.ProjectId = activityOld.ProjectId;
                TimeSheetContext.Entry(activityOld).State = EntityState.Detached;
                activityOld = null;
                TimeSheetContext.Update(activity);
            }
            catch (Exception e)
            {
                return false;
            }
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }
    }
}