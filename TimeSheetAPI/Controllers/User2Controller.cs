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
    public class User2Controller : ControllerBase
    {
        TimeSheetContext timeSheetContext;
        public void TimeSheetContext(TimeSheetContext timeSheetContext)
        {
            this.timeSheetContext = timeSheetContext;
        }
        [HttpPost("Login")]
        public Dto.User Login([FromBody] Dto.User input)
        {
            if (input.Email == "anthe.boets@student.ehb.be" && input.Password == "123")
            {
                ICollection<Dto.Log> Logs = new List<Dto.Log>();
                Logs.Add(new Dto.Log { Id = 2, Start = new DateTime().AddMilliseconds(1483254000), Stop = new DateTime().AddMilliseconds(1483270200), Description = "coding-frontend" });
                Logs.Add(new Dto.Log { Id = 4, Start = new DateTime().AddMilliseconds(1483342200), Stop = new DateTime().AddMilliseconds(1483363800), Description = "coding-backend" });
                Logs.Add(new Dto.Log { Id = 5, Start = new DateTime().AddMilliseconds(1483428600), Stop = new DateTime().AddMilliseconds(1483457400), Description = "coding-database" });
                return new Dto.User { Id = 1, Name = "anthe", Email = "anthe.boets@student.ehb.be", Password = "123", Logs = Logs };
            }
            if (input.Email == "anthe2.boets@student.ehb.be" && input.Password == "123")
            {
                ICollection<Dto.Log> Logs = new List<Dto.Log>();
                Logs.Add(new Dto.Log { Id = 8, Start = new DateTime().AddMilliseconds(1483255800), Stop = new DateTime().AddMilliseconds(1483277400), Description = "coding html" });
                Logs.Add(new Dto.Log { Id = 10, Start = new DateTime().AddMilliseconds(1483342200), Stop = new DateTime().AddMilliseconds(1483360200), Description = "coding js" });
                return new Dto.User { Id = 2, Name = "anthe2", Email = "anthe2.boets@student.ehb.be", Password = "123", Logs = Logs };
            }
            /*
            Models.User User = timeSheetContext.User.Single(x => x.Email == input.Email && x.Password == input.Password);
            ICollection<Dto.Log> Logs = new List<Dto.Log>();
            foreach (var log in User.Logs)
            {
                Logs.Add(new Dto.Log { Id = log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description });
            }
            
            return new Dto.User { Id = User.Id, Name = User.Name, Email = User.Email, Password = User.Password, Logs = Logs };
            */
            return new Dto.User();
        }

        [HttpPost("Get")]
        public Dto.User GetById([FromBody] Dto.User input)
        {

            //Models.User User = timeSheetContext.User.Where(x => x.Id == input.Id);
            /*
            ICollection<Dto.Log> Logs = new List<Dto.Log>();
            foreach (var log in User.Logs)
            {
                Logs.Add(new Dto.Log { Id=log.Id, Start = log.Start, Stop = log.Stop, Description = log.Description});
            }
            
            return new Dto.User { Id=User.Id, Name=User.Name, Email=User.Email, Password=User.Password, Logs=Logs };
            */
            if (input.Id == 1)
            {
                ICollection<Dto.Log> Logs = new List<Dto.Log>();
                Logs.Add(new Dto.Log { Id = 2, Start = new DateTime().AddMilliseconds(1483254000), Stop = new DateTime().AddMilliseconds(1483270200), Description = "coding-frontend" });
                Logs.Add(new Dto.Log { Id = 4, Start = new DateTime().AddMilliseconds(1483342200), Stop = new DateTime().AddMilliseconds(1483363800), Description = "coding-backend" });
                Logs.Add(new Dto.Log { Id = 5, Start = new DateTime().AddMilliseconds(1483428600), Stop = new DateTime().AddMilliseconds(1483457400), Description = "coding-database" });
                return new Dto.User { Id = 1, Name = "anthe", Email = input.Email, Password = input.Password, Logs = Logs };
            }
            if (input.Id == 2)
            {
                ICollection<Dto.Log> Logs = new List<Dto.Log>();
                Logs.Add(new Dto.Log { Id = 8, Start = new DateTime().AddMilliseconds(1483255800), Stop = new DateTime().AddMilliseconds(1483277400), Description = "coding html" });
                Logs.Add(new Dto.Log { Id = 10, Start = new DateTime().AddMilliseconds(1483342200), Stop = new DateTime().AddMilliseconds(1483360200), Description = "coding js" });
                return new Dto.User { Id = 2, Name = "anthe2", Email = input.Email, Password = input.Password, Logs = Logs };
            }
            return new Dto.User();

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
