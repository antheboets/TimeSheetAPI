﻿using System;
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
        [HttpPost]
        public Dto.User Login(String email, String password)
        {
            return new Dto.User { Name = "test", Email=email, Password=password };
        }

        [HttpGet("{id}")]
        public ICollection<Dto.Log> GetLogs(int Id)
        {
            //ICollection<Dto.Log> Logs = new ICollection<Dto.Log>;
            return null;

        }
        [HttpGet]
        public Dto.User Loginss()
        {
            return new Dto.User { Name = "test"};
        }
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
