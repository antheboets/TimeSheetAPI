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
            if (log.ProjectId == null  || log.ProjectId == "")
            {
                return BadRequest("no projectId");
            }
            Models.Project project = await TimeSheetContext.Project.Where(x => x.Id== log.ProjectId).SingleOrDefaultAsync();
            if (project == null)
            {
                return BadRequest("no projectId");
            }
            Models.Log ModelLog = new Models.Log { Start=log.Start, Stop=log.Stop, Description=log.Description, UserId= User.FindFirst(ClaimTypes.NameIdentifier).Value, Project = project };
            if (log.ActivityId != null || log.ActivityId != "")
            {
                Models.Activity activity = await TimeSheetContext.Activity.Where(x => x.Id == log.ActivityId).SingleOrDefaultAsync();
                if (activity != null)
                {
                    ModelLog.Activity = activity;
                }
            }
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
            Dto.Log logDto = new Dto.Log{ Id = log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description, ActivityId = log.ActivityId, ProjectId = log.ProjectId, UserId = log.UserId }; 

            return logDto;
        }
        [AllowAnonymous]
        [HttpGet("Test")]
        public async Task<List<Dto.Log>> Get()
        {
            var log = await TimeSheetContext.Log.ToListAsync();
            List<Dto.Log> logList = new List<Log>();
            foreach (var item in log)
            {
                logList.Add(new Dto.Log { Id = item.Id, Start = item.Start, Stop = item.Stop, Description = item.Description, ActivityId=item.ActivityId, ProjectId=item.ProjectId, UserId=item.UserId });
            }
            return logList;
        }
        [HttpPost("Get")]
        public async Task<ICollection<Dto.Log>> Get([FromBody] Dto.TimeObject CurrentTimeObj)
        {
            DateTime CurrentTime;
            if (CurrentTimeObj == null)
            {
                CurrentTime = DateTime.Now;
            }
            else
            {
                CurrentTime = CurrentTimeObj.Time;
            }
            CurrentTimeObj.Time = CurrentTimeObj.Time.Date;
            var thisWeekStart = CurrentTime.AddDays(-(int)CurrentTime.DayOfWeek + 1);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            List<Models.Log> logs = await TimeSheetContext.Log.Where(x => x.Start > thisWeekStart).Where(x => x.Stop < thisWeekEnd).Where(x => x.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value).ToListAsync();
            List<Dto.Log> DtoLogs = new List<Dto.Log>();
            foreach (var log in logs)
            {
                DtoLogs.Add(new Dto.Log { Id = log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description, ActivityId = log.ActivityId, ProjectId = log.ProjectId, UserId = log.UserId } );
            }
            return DtoLogs;
        }
        [HttpGet("GetAll")]
        public async Task<ICollection<Dto.Log>> GetAll()
        {
            var logs = await TimeSheetContext.Log.Where(x => x.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value).ToListAsync();

            List<Dto.Log> DtoLogs = new List<Dto.Log>();

            foreach (var log in logs)
            {
                DtoLogs.Add(new Dto.Log { Id = log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description, ActivityId = log.ActivityId, ProjectId = log.ProjectId, UserId = log.UserId });
            }
            return DtoLogs;
        }
        //todo get logs dynamic scrolling 20 per request
        [HttpGet("GetList")]
        public async Task<ICollection<Dto.Log>> GetList([FromQuery]int Page = 0)
        {
            if (Page < 0)
            {
                Page = 0;
            }
            var Logs = await TimeSheetContext.Log.Skip(Page * 20).Take((Page + 1) * 20).OrderBy(x => x.Start).ToListAsync();
            List<Dto.Log> DtoLogs = new List<Dto.Log>();
            foreach (var log in Logs)
            {
                DtoLogs.Add(new Dto.Log { Id = log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description, ActivityId = log.ActivityId, ProjectId = log.ProjectId, UserId = log.UserId });
            }
            return DtoLogs;
        }
    }
}