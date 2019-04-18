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
    public class ActivityController
    {
        private readonly TimeSheetContext timeSheetContext;

        public ActivityController(TimeSheetContext timeSheetContext)
        {
            this.timeSheetContext = timeSheetContext;
        }
        [AllowAnonymous]
        [HttpGet("Test")]
        public async Task<List<Dto.Activity>> Test()
        {
            List<Models.Activity> Activities = await timeSheetContext.Activity.ToListAsync();
            List<Activity> activitiesDto = new List<Dto.Activity>();
            foreach (var activity in Activities)
            {
                activitiesDto.Add(new Activity { Id= activity.Id, Name= activity.Name, ProjectId=activity.ProjectId});
            }
            return activitiesDto;
        }
    }
}
