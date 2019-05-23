using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Infrastructure
{
    public class LogRepository : ILogRepository
    {
        private readonly TimeSheetContext TimeSheetContext;
        public LogRepository(TimeSheetContext timeSheetContext)
        {
            TimeSheetContext = timeSheetContext;
        }
        public async Task<bool> Create (Models.Log log)
        {
            if (!IsValidLog(log))
            {
                return false;
            }
            int currentLogs = (DateTime.Now.Year * 12) + DateTime.Now.Month;
            if ((log.Start.Year * 12) + log.Start.Month != currentLogs || (log.Stop.Year * 12) + log.Stop.Month != currentLogs)
            {
                return false;
            }
            Models.Project project = await TimeSheetContext.Project.Where(x => x.Id == log.ProjectId).SingleOrDefaultAsync();
            if (project == null)
            {
                return false;
            }
            if (log.ActivityId != null || log.ActivityId != "")
            {
                Models.Activity activity = await TimeSheetContext.Activity.Where(x => x.Id == log.ActivityId).SingleOrDefaultAsync();
                if (activity != null && activity.ProjectId == log.ProjectId)
                {
                    log.Activity = activity;
                }
            }
            await TimeSheetContext.AddAsync(log);
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Delete(Models.Log log)
        {
            log = await Get(log);
            if (log == null)
            {
                return false;
            }
            TimeSheetContext.Remove(log);
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }
        public async Task<Models.Log> Get(Models.Log log)
        {
            if (log.Id == null)
            {
                return null;
            }
            if (log.Id == "")
            {
                return null;
            }
            if (log.UserId == null)
            {
                return null;
            }
            if (log.UserId == "")
            {
                return null;
            }
            try
            {
                return await TimeSheetContext.Log.Where(x => x.Id == log.Id && x.UserId == log.UserId).SingleOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<Log>> GetAll()
        {
            return await TimeSheetContext.Log.ToListAsync();
        }

        public async Task<List<Log>> GetAllOfUser(string userId)
        {
            if (userId == null)
            {
                return null;
            }
            if (userId == "")
            {
                return null;
            }
            return await TimeSheetContext.Log.Where(x => x.UserId == userId).ToListAsync();
        }
        public async Task<List<Models.Log>> GetDay(DateTime day, string userId)
        {
            if (day == null)
            {
                day = DateTime.Now;
            }
            List<Models.Log> list = await TimeSheetContext.Log.Where(x => x.Start.Day == day.Day && x.Stop.Day == day.Day).ToListAsync();
            if ( list == null)
            {
                return null;
            }
            return list;
        }
        public async Task<List<Log>> GetDynamicScroll(int page)
        {
            return await TimeSheetContext.Log.Skip(page * 20).Take((page + 1) * 20).OrderBy(x => x.Start).ToListAsync(); ;
        }
        public async Task<Log> GetForHr(Log log)
        {
            if (log.Id == null)
            {
                return null;
            }
            if (log.Id == "")
            {
                return null;
            }
            try
            {
                return await TimeSheetContext.Log.Where(x => x.Id == log.Id).SingleOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<Log>> GetWeek(DateTime WeekStart, DateTime WeekStop, string userId)
        {
            return await TimeSheetContext.Log.Where(x => x.Start > WeekStart && x.Stop < WeekStop && x.UserId == userId).ToListAsync();
        }
        public bool IsCurrentMonth(Models.Log log)
        {
            int currentMonth = (DateTime.Now.Year * 12) + DateTime.Now.Month;
            if (currentMonth == (log.Start.Year * 12) + log.Start.Month && currentMonth == (log.Stop.Year * 12) + log.Stop.Month)
            {
                return true;
            }
            return false;
        }
        public bool IsValidLog(Models.Log log)
        {
            if (log == null)
            {
                return false;
            }
            if (log.ProjectId == null || log.ProjectId == "")
            {
                return false;
            }
            return true;
        }
        public bool StartStopInSameMonth(Models.Log log)
        {
            if ((log.Start.Year * 12) + log.Start.Month == (log.Stop.Year * 12) + log.Stop.Month)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> Update(Models.Log log)
        {
            if (IsValidLog(log))
            {
                return false;
            }
            TimeSheetContext.Update(log);
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }
    }
}