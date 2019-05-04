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
    public class UserController : ControllerBase
    {
        TimeSheetContext TimeSheetContext;
        public UserController(TimeSheetContext TimeSheetContext)
        {
            this.TimeSheetContext = TimeSheetContext;
        }
        [HttpPost("Get")]
        public async Task<Dto.User> GetById([FromBody] Dto.User input)
        {

            if (input.Id == "")
            {
                return new Dto.User();
            }
            Models.User User = await TimeSheetContext.User.Include(x => x.Logs).SingleAsync(x => x.Id == input.Id);
            ICollection<Dto.Log> Logs = new List<Dto.Log>();
            if (User != null)
            {
                foreach (var log in User.Logs)
                {
                    Logs.Add(new Dto.Log { Id = log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description });
                }
                return new Dto.User { Id = User.Id, Name = User.Name, Email = User.Email, Logs = Logs };
            }
            else
            {
                return new Dto.User();
            }
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Update([FromBody]Dto.UserForUpdate userForUpdate)
        {
            //var model = new User { Id = , Email=userForUpdate.Email, Name= userForUpdate.Name};
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != userForUpdate.Id)
            {
                return BadRequest();
            }
            var model = new User { Id = userForUpdate.Id , Email = userForUpdate.Email, Name = userForUpdate.Name };
            TimeSheetContext.Update(model);
            await TimeSheetContext.SaveChangesAsync();
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<Dto.User> Test()
        {
            
            Models.User User = await TimeSheetContext.User.FirstAsync();

            return new Dto.User { Id = User.Id, Name = User.Name, Email = User.Email, Role=User.Role}; 
            
        }
    }
}
