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
    public class LogController : ControllerBase
    {
        TimeSheetContext TimeSheetContext;
        public LogController(TimeSheetContext TimeSheetContext)
        {
            this.TimeSheetContext = TimeSheetContext;
        }
        [HttpPost("Create")]
        public void Create(Dto.Log log)
        {

        }
        [HttpGet("Get")]
        public async Task<Dto.Log> Get()
        {
              
            return null;
        }
        [HttpGet("GetTest")]
        public async Task<Dto.Log> GetTest(int id)
        {
            Models.Log log = await TimeSheetContext.Log.SingleAsync(x => x.Id == id);
            
            return new Dto.Log{ Id=log.Id, Start=log.Start, Description=log.Description,Stop=log.Stop};
        }
    }
}
