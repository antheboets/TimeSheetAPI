using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace TimeSheetAPI.Infrastructure
{
    public class LogRepo : ILogRepository
    {
        public Task<bool> IsCurrentMond(Log log)
        {
            if (log.Start.Month == DateTime.Now.Month)
            {
                //return true;
            }
            //return false;
            throw new NotImplementedException();
        }
    }
}
