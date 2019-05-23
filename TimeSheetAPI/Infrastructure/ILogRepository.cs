using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSheetAPI.Infrastructure
{
    public interface ILogRepository
    {
        bool StartStopInSameMonth(Models.Log log);
        bool IsCurrentMonth(Models.Log log);
        Task<bool> Create (Models.Log log);
        bool IsValidLog(Models.Log log);
        Task<bool> Delete(Models.Log log);
        Task<bool> Update(Models.Log log);
        Task<Models.Log> Get(Models.Log log);
        Task<Models.Log> GetForHr(Models.Log log);
        Task<List<Models.Log>> GetAll();
        Task<List<Models.Log>> GetWeek(DateTime WeekStart, DateTime WeekStop, string userId);
        Task<List<Models.Log>> GetAllOfUser(string userId);
        Task<List<Models.Log>> GetDynamicScroll(int page);
        Task<List<Models.Log>> GetDay(DateTime Day, string userId);
    }
}