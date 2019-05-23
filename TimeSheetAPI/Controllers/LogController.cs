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
using System.Net;
using Microsoft.Extensions.Configuration;

namespace TimeSheetAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogRepository Repo;
        private readonly IConfiguration Config;
        
        public LogController(ILogRepository Repo,IConfiguration Config)
        {
            this.Repo = Repo;
            this.Config = Config;
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody]Dto.LogForCreate log)
        {
            Models.Log ModelLog = new Models.Log { UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value, Start = log.Start, Stop = log.Stop, Description = log.Description, ProjectId  = log.ProjectId , ActivityId = log.ActivityId};
            if (await Repo.Create(ModelLog))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([FromBody] Dto.LogForDelete log)
        {
            Models.Log ModelLog = new Models.Log { Id = log.Id, UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value };
            if (await Repo.Delete(ModelLog))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Update([FromBody]Dto.LogForUpdate log)
        {
            Models.Log ModelLog = new Models.Log { Id=log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description, UserId = log.UserId ,ActivityId = log.ActivityId };
            if (await Repo.Update(ModelLog))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("Get")]
        public async Task<ActionResult<Dto.Log>> Get([FromQuery] string Id)
        {
            Models.Log log = null;
            if (User.FindFirst(ClaimTypes.Role).Value == Config.GetSection("Role:Consultant:Name").Value)
            {
                log = new Models.Log { Id = Id, UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value };
                if (log == null)
                {
                    return BadRequest();
                }
                log = await Repo.Get(log);
            }
            else
            {
                log = new Models.Log { Id = Id };
                if (log == null)
                {
                    return BadRequest();
                }
                log = await Repo.GetForHr(log);
            }
            if (log == null)
            {
                return BadRequest();
            }
            return Ok(new Dto.Log { Id = log.Id, Start = log.Start, Stop = log.Stop, Description=log.Description, ActivityId = log.ActivityId, UserId = log.ActivityId, ProjectId = log.ProjectId });
        }
        [AllowAnonymous]
        [HttpGet("Test")]
        public async Task<ActionResult<List<Dto.Log>>> Get()
        {
            List<Dto.Log> DtoLogs = new List<Dto.Log>();
            List<Models.Log> logs = await Repo.GetAll();
            foreach (Models.Log item in logs)
            {
                DtoLogs.Add(new Dto.Log { Id=item.Id, Start=item.Start, Stop = item.Stop, Description=item.Description, UserId=item.UserId, ActivityId=item.ActivityId, ProjectId = item.ProjectId });
            }
            return Ok(DtoLogs);
        }
        [HttpPost("GetWeek")]
        public async Task<ActionResult<ICollection<Dto.Log>>> GetWeek([FromBody] Dto.TimeObject CurrentTimeObj)
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
            DateTime thisWeekStart = CurrentTime.AddDays(-(int)CurrentTime.DayOfWeek + 1);
            DateTime thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            List<Models.Log> logs = await Repo.GetWeek(thisWeekStart, thisWeekEnd, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<Dto.Log> DtoLogs = new List<Dto.Log>();
            foreach (var log in logs)
            {
                DtoLogs.Add(new Dto.Log { Id = log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description, ActivityId = log.ActivityId, ProjectId = log.ProjectId, UserId = log.UserId } );
            }
            return Ok(DtoLogs);
        }
        [HttpGet("GetAllOfUser")]
        public async Task<ActionResult<ICollection<Dto.Log>>> GetAllOfUser()
        {
            var logs = await Repo.GetAllOfUser(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<Dto.Log> DtoLogs = new List<Dto.Log>();
            foreach (var log in logs)
            {
                DtoLogs.Add(new Dto.Log { Id = log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description, ActivityId = log.ActivityId, ProjectId = log.ProjectId, UserId = log.UserId });
            }
            return Ok(DtoLogs);
        }
        //todo get logs dynamic scrolling 20 per request
        [HttpGet("GetList")]
        public async Task<ActionResult<ICollection<Dto.Log>>> GetList([FromQuery]int Page = 0)
        {
            if (Page < 0)
            {
                Page = 0;
            }
            var Logs = await Repo.GetDynamicScroll(Page);
            List<Dto.Log> DtoLogs = new List<Dto.Log>();
            foreach (var log in Logs)
            {
                DtoLogs.Add(new Dto.Log { Id = log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description, ActivityId = log.ActivityId, ProjectId = log.ProjectId, UserId = log.UserId });
            }
            return Ok(DtoLogs);
        }
    }
}