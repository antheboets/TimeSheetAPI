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
        TimeSheetContext timeSheetContext;
        public void TimeSheetContext(TimeSheetContext timeSheetContext)
        {
            this.timeSheetContext = timeSheetContext;
        }
        [HttpPost("Login")]
        public Dto.User Login([FromBody] Dto.User input)
        {
            /*
            Models.User User = timeSheetContext.User.Single(x => x.Email == input.Email && x.Password == input.Password);
            ICollection<Dto.Log> Logs = new List<Dto.Log>();
            foreach (var log in User.Logs)
            {
                Logs.Add(new Dto.Log { Id = log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description });
            }
            
            return new Dto.User { Id = User.Id, Name = User.Name, Email = User.Email, Password = User.Password, Logs = Logs };
            */
            return input;
        }

        [HttpPost("Get")]
        public Dto.User GetById([FromBody] Dto.User input)
        {
            
            Models.User User = timeSheetContext.User.Single(x => x.Id == input.Id);
            /*
            ICollection<Dto.Log> Logs = new List<Dto.Log>();
            foreach (var log in User.Logs)
            {
                Logs.Add(new Dto.Log { Id=log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description});
            }
            
            return new Dto.User { Id=User.Id, Name=User.Name, Email=User.Email, Password=User.Password, Logs=Logs };
            */
            return new Dto.User { Id = User.Id};
        }
        /*
        [HttpGet]
        public Dto.User Test()
        {

            return new Dto.User { Name = "test"};
        }
        */
        /*
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
