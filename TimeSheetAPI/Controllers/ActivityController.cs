using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSheetAPI.Infrastructure;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityRepository Repo;
        public ActivityController(IActivityRepository Repo)
        {
            this.Repo = Repo;
        }
        [AllowAnonymous]
        [HttpGet("Test")]
        public async Task<ActionResult<IEnumerable<Dto.Activity>>> Test()
        {
            List<Models.Activity> activities = await Repo.GetAll();
            List<Dto.Activity> activitiesDto = new List<Dto.Activity>();
            foreach (Models.Activity activity in activities) 
            {
                activitiesDto.Add(new Dto.Activity { Id = activity.Id, Name= activity.Name, ProjectId= activity.ProjectId });
            }
            return Ok(activitiesDto);
        }
        [HttpGet("Get")]
        public async Task<ActionResult<Dto.Activity>> GetActivity([FromQuery]string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (id == "")
            {
                return BadRequest();
            }
            Models.Activity activity = new Models.Activity();
            activity = await Repo.Get(activity);
            if (activity == null)
            {
                return NotFound();
            }
            return Ok(activity);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> PutActivity([FromBody]Dto.ActivityForCreate activity)
        {
            if (activity == null)
            {
                return BadRequest();
            }
            if (activity.ProjectId == null)
            {
                return BadRequest();
            }
            if (activity.ProjectId == "")
            {
                return BadRequest();
            }
            Models.Activity activityModel = new Models.Activity {Name=activity.Name, ProjectId=activity.ProjectId};
            if (await Repo.Create(activityModel))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("Update")]
        public async Task<ActionResult> PostActivity([FromBody]Dto.ActivityForUpdate activity)
        {
            if (activity == null)
            {
                return BadRequest();
            }
            if (activity.Id == null)
            {
                return BadRequest();
            }
            if (activity.Id == "")
            {
                return BadRequest();
            }
            Models.Activity activityModel = new Models.Activity {Id = activity.Id, Name = activity.Name};
            if (await Repo.Update(activityModel))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([FromBody] Dto.ActivityForDelete activity)
        {
            if (activity == null)
            {
                return BadRequest();
            }
            if (activity.Id == null)
            {
                return BadRequest();
            }
            if (activity.Id == "")
            {
                return BadRequest();
            }
            Models.Activity activityModel = new Models.Activity { Id = activity.Id};
            if (await Repo.Delete(activityModel))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}