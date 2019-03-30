using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetAPI.Models;
namespace TimeSheetAPI.Infrastructure
{
    interface ILogRepository
    {
        Task<bool> IsCurrentMond(Log log);
    }
}
