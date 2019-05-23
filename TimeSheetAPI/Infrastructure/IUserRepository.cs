using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Infrastructure
{
    public interface IUserRepository
    {
        Task<List<Models.User>> GetAllConsultant();
        Task<bool> UpdateWorkMonth(Models.WorkMonth workMonth);
        Task<Models.User> Get(Models.User user);
        Task<List<string>> GetListOfLogs(Models.User user);
        Task<List<string>> GetListOfExceptionDays(Models.User user);
        Task<bool> Update(Models.User user);
        Task<bool> Delete (Models.User user);
        Task<List<Models.User>> GetAll();
        string GetTotalTime(Models.User user);
        string GetSalary(Models.User user);
        Task<List<Models.WorkMonth>> GetAllWorkMonths();
        Task<Models.WorkMonth> GetWorkMonths(Models.User user, DateTime Time);
        Task<Models.DefaultWorkweek> GetDefaultWorkweek(Models.DefaultWorkweek defaultWorkweek);
        Task<bool> UpdateDefaultWorkWeek(Models.DefaultWorkweek defaultWorkweek);
        Task<List<string>> GetAllMails();
        Task<List<Models.Log>> GetLogsOfUserMonth(Models.User user, DateTime Time);
        List<string> LogsToIds(ICollection<Models.Log> logs);
    }
}
