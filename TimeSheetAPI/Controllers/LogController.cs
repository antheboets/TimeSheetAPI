using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Dto;
using TimeSheetAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TimeSheetAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        TimeSheetContext TimeSheetContext;
        public LogController(TimeSheetContext TimeSheetContext)
        {
            this.TimeSheetContext = TimeSheetContext;
        }
        [HttpPost("Create")]
        public async void Create(Dto.Log log)
        {
            Models.Log ModelLog = new Models.Log {Id=log.Id, Start=log.Start, Stop=log.Stop, Description=log.Description };
            await TimeSheetContext.Log.AddAsync(ModelLog);
        }
        [HttpGet("Get")]
        public async Task<Dto.Log> Get()
        {     
            return null;
        }
        [HttpPost("Get")]
        public async Task<Dto.Log> Get([FromBody] long currentTimeTicks)
        {
            //https://stackoverflow.com/questions/2821035/c-sharp-get-start-date-and-last-date-based-on-current-date
            DateTime currentTime = new DateTime(currentTimeTicks);
            currentTime = currentTime.Date;
            DateTime ns = new DateTime();
            var thisWeekStart = currentTime.AddDays(-(int)currentTime.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);


            return null;
        }
    }
}
