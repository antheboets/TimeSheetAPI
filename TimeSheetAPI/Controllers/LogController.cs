using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TimeSheetAPI.Dto;
using TimeSheetAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TimeSheetAPI.Controllers
{
    [Authorize]
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
        public async Task<ActionResult> Create(Dto.LogForCreate log)
        {
            Models.Log ModelLog = new Models.Log { Start=log.Start, Stop=log.Stop, Description=log.Description, UserId= User.FindFirst(ClaimTypes.NameIdentifier).Value, Activity= new Models.Activity(), Project = new Models.Project() };
            await TimeSheetContext.AddAsync(ModelLog);
            await TimeSheetContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([FromBody] Dto.LogForDelete log)
        {
            Models.Log ModelLog = await TimeSheetContext.Log.Where(x => x.Id == log.Id).Where(x => x.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value).SingleOrDefaultAsync();
            if (ModelLog == null)
            {
                BadRequest();
            }
            TimeSheetContext.Remove(ModelLog);
            await TimeSheetContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Update(Dto.LogForUpdate log)
        {
            Models.Log ModelLog = new Models.Log { Id=log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description, UserId = log.UserId };
            TimeSheetContext.Update(ModelLog);
            await TimeSheetContext.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("Get")]
        public async Task<Dto.Log> Get([FromQuery] String Id)
        {
            Models.Log log = await TimeSheetContext.Log.Where(x => x.Id == Id).Where(x => x.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefaultAsync();
            if (log == null)
            {
                return null;
            }
            Dto.Log logDto = new Dto.Log{ Id= log.Id, Start = log.Start, Stop= log.Stop, Description = log.Description }; 

            return logDto;
        }
        [HttpGet("Test")]
        public async Task<Dto.Log> Get()
        {
            var log = await TimeSheetContext.Log.Where(x => x.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefaultAsync();
            Dto.Log DtoLog = new Dto.Log { Id = log.Id, Start=log.Start, Stop=log.Stop, Description=log.Description };
            return DtoLog;
        }
        [HttpPost("Get")]
        public async Task<ICollection<Dto.Log>> Get([FromBody] Dto.TimeObject CurrentTimeObj)
        {

            DateTime CurrentTime = CurrentTimeObj.Time;
            CurrentTimeObj.Time = CurrentTimeObj.Time.Date;
            var thisWeekStart = CurrentTimeObj.Time.AddDays(-(int)CurrentTimeObj.Time.DayOfWeek + 1);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            List<Models.Log> logs = await TimeSheetContext.Log.Where(x => x.Start > thisWeekStart).Where(x => x.Stop < thisWeekEnd).Where(x => x.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value).ToListAsync();
            List<Dto.Log> DtoLogs = new List<Dto.Log>();
            foreach (var log in logs)
            {
                DtoLogs.Add(new Dto.Log { Id = log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description });
            }
            return DtoLogs;
        }
    }
}
