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
    public class UserController : ControllerBase
    {
        TimeSheetContext TimeSheetContext;
        public UserController(TimeSheetContext TimeSheetContext)
        {
            this.TimeSheetContext = TimeSheetContext;
        }
        [HttpPost("Login")]
        public async Task<Dto.User> Login([FromBody] Dto.User input)
        {
            
            Models.User User = await TimeSheetContext.User.Include(x => x.Logs).SingleAsync(x => x.Email == input.Email && x.Password == input.Password);
            ICollection<Dto.Log> Logs = new List<Dto.Log>();
            foreach (var log in User.Logs)
            {
                Logs.Add(new Dto.Log { Id = log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description });
            }

            return new Dto.User { Id = User.Id, Name = User.Name, Email = User.Email, Password = User.Password, Logs = Logs };
        }

        [HttpPost("Get")]
        public async Task<Dto.User> GetById([FromBody] Dto.User input)
        {
            
            Models.User User =  await TimeSheetContext.User.Include(x => x.Logs).SingleAsync(x => x.Id == input.Id);
            
            ICollection<Dto.Log> Logs = new List<Dto.Log>();
            foreach (var log in User.Logs)
            {
                Logs.Add(new Dto.Log { Id=log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description});
            }
            
            return new Dto.User { Id=User.Id, Name=User.Name, Email=User.Email, Password=User.Password, Logs=Logs };
        }
        [HttpGet("test")]
        public async Task<Dto.User> Test()
        {
            Models.User User = await TimeSheetContext.User.FirstAsync();

            return new Dto.User { Id = User.Id, Name = User.Name, Email = User.Email, Password = User.Password}; 

        }
    }
}
