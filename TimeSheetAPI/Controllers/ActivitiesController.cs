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
    [Route("controller")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly TimeSheetContext Repo;

        public ActivitiesController(TimeSheetContext repo)
        {
            Repo = repo;
        }

        // GET: api/Activities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivity()
        {
            return await Repo.Activity.ToListAsync();
        }

        // GET: api/Activities/5
        [HttpGet("Get")]
        public async Task<ActionResult<Activity>> GetActivity(string id)
        {
            var activity = await Repo.Activity.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }

            return activity;
        }

        // PUT: api/Activities/5
        [HttpPost("")]
        public async Task<IActionResult> PutActivity(string id, Activity activity)
        {
            if (id != activity.Id)
            {
                return BadRequest();
            }

            Repo.Entry(activity).State = EntityState.Modified;

            try
            {
                await Repo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Activities
        [HttpPost]
        public async Task<ActionResult<Activity>> PostActivity(Activity activity)
        {
            Repo.Activity.Add(activity);
            await Repo.SaveChangesAsync();

            return CreatedAtAction("GetActivity", new { id = activity.Id }, activity);
        }

        // DELETE: api/Activities/5
        [HttpDelete("Delete")]
        public async Task<ActionResult<Activity>> DeleteActivity([FromBody] Dto.ActivityForDelete activityForDelete)
        {
            var activity = await Repo.Activity.FindAsync(activityForDelete.Id);
            if (activity == null)
            {
                return NotFound();
            }

            Repo.Activity.Remove(activity);
            await Repo.SaveChangesAsync();

            return activity;
        }

        private bool ActivityExists(string id)
        {
            return Repo.Activity.Any(e => e.Id == id);
        }
    }
}
