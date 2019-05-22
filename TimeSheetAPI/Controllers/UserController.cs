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
using Microsoft.Extensions.Configuration;
using AutoMapper;

namespace TimeSheetAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository Repo;
        private readonly IConfiguration Config;
        private readonly IMapper Mapper;

        public UserController(IUserRepository Repo, IConfiguration Config, IMapper Mapper)
        {
            this.Repo = Repo;
            this.Config = Config;
            this.Mapper = Mapper;
        }
        [HttpPost("Get")]
        public async Task<Dto.UserForGet> Get([FromBody] Dto.UserForGet UserId)
        {
            if (UserId.Id == "")
            {
                return null;
            }
            Models.User user = new Models.User { Id = UserId.Id };
            /*  
            var userModel = Task.Run(() => Repo.Get(user));
            var ExceptionDaysIds = Task.Run(() => Repo.GetListOfExceptionDays(user));
            var LogsIds = Task.Run(() => Repo.GetListOfLogs(user));
            await Task.WhenAll(userModel, ExceptionDaysIds, LogsIds);
            */
            var userModel = await Repo.Get(user);
            var ExceptionDaysIds = await Repo.GetListOfExceptionDays(user);
            var LogsIds = await Repo.GetListOfLogs(user);
            if (userModel == null)
            {
                return null;
            }
            Dto.UserForGet userForGet = new Dto.UserForGet { Id= userModel.Id , Name= userModel.Name, ChangeHistory= userModel.ChangeHistory, Email= userModel.Email, DefaultWorkweekId= userModel.DefaultWorkweekId, RoleId= userModel.RoleId, ExceptionWorkDayIds= ExceptionDaysIds, LogIds=LogsIds};
            //Mapper.Map<Dto.UserForGet>(userModel.Result);
            if (userForGet == null)
            {
                return null;
            }
            if (userForGet.Id == "")
            {
                return null;
            }
            //return new Dto.UserForGet { Id = userModel.Result.Id, Name = userModel.Result.Name, Email = userModel.Result.Email, LogIds = LogsIds.Result, ChangeHistory = userModel.Result.ChangeHistory, RoleId = userModel.Result.RoleId, DefaultWorkweekId = userModel.Result.DefaultWorkweekId, ExceptionWorkDayIds = ExceptionDaysIds.Result };
            if (ExceptionDaysIds != null)
            {
                userForGet.ExceptionWorkDayIds = ExceptionDaysIds;
            }
            if (LogsIds != null)
            {
                userForGet.LogIds = LogsIds;
            }
            return userForGet;
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Update([FromBody]Dto.UserForUpdate userForUpdate)
        {
            //var model = new User { Id = , Email=userForUpdate.Email, Name= userForUpdate.Name};
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != userForUpdate.Id)
            {
                return Unauthorized();
            }
            if (userForUpdate == null)
            {
                return BadRequest();
            }
            Models.User user = Mapper.Map<Models.User>(userForUpdate);
            if (await Repo.Update(user))
            {
                return Ok();
            }
            return BadRequest();
        }
        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<List<Dto.UserForGetFull>> Test()
        {
            return Mapper.Map<List<Dto.UserForGetFull>> (await Repo.GetAll());
        }
        //HR
        [HttpGet("GetConsultants")]
        public async Task<List<Dto.UserForGetHR>> GetConsultants()
        {

            if (User.FindFirst(ClaimTypes.Role).Value != Config.GetSection("Role:Human-Resources").Value)
            {
                Unauthorized();
            }
            /*
            var users = Task.Run(()=> Repo.GetAllConsultant());
            var workMonths = Task.Run(() => Repo.GetAllWorkMonths());
            await Task.WhenAll(users, workMonths);
            */
            var users = await Repo.GetAllConsultant();
            var workMonths = await Repo.GetAllWorkMonths();
            List<Dto.UserForGetHR> userDto = new List<Dto.UserForGetHR>();
            if (users == null)
            {
                BadRequest();
            }
            foreach (Models.User user in users)
            {

                Dto.UserForGetHR userForGetHR = new UserForGetHR { Id = user.Id, Email = user.Email, Name = user.Name, ChangeHistory = user.ChangeHistory };
                //Mapper.Map<Dto.UserForGetHR>(user);
                try
                {
                    Models.WorkMonth workMonthModel = workMonths.Where(x => x.Id == user.Id).Single();
                    Dto.WorkMonth workMonth = new WorkMonth { Id = workMonthModel.Id, Month = workMonthModel.Month, Accepted = workMonthModel.Accepted, UserId = workMonthModel.UserId };
                    //Mapper.Map<Dto.WorkMonth>(workMonths.Result.Where(x => x.UserId == user.Id));
                    workMonth.Salary = Repo.GetSalary(user);
                    workMonth.TotalHours = Repo.GetTotalTime(user);
                    userForGetHR.WorkMonth = workMonth;
                    
                }
                catch (Exception e)
                {

                }
                userDto.Add(userForGetHR);
            }
            return userDto;
        }
        [HttpPost("UpdateWorkMonth")]
        public async Task<ActionResult> UpdateUser(Dto.WorkMontForUpdate workMonth)
        {
            
            if (workMonth == null)
            {
                return BadRequest();
            }
            Models.WorkMonth workMonthModel = new Models.WorkMonth { Id = workMonth.Id, Accepted = workMonth.Accepted, Month = workMonth.Month, UserId = workMonth.UserId };
            if (await Repo.UpdateWorkMonth(workMonthModel))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("GetDefaultWorkWeek")]
        public async Task<Dto.DefaultWorkweek> GetDefaultWorkWeek([FromQuery] string Id)
        {
            if (Id == null)
            {
                return null;
            }
            if (Id == "")
            {
                return null;
            }
            Models.DefaultWorkweek defaultWorkweekModel = new Models.DefaultWorkweek { Id = Id };
            defaultWorkweekModel = await Repo.GetDefaultWorkweek(defaultWorkweekModel);
            if (defaultWorkweekModel == null)
            {
                return null;
            }
            return Mapper.Map <Dto.DefaultWorkweek>(defaultWorkweekModel);//new Dto.DefaultWorkweek { Id = defaultWorkweekModel.Id, Monday = defaultWorkweekModel.Monday};
        }
        [HttpPost("UpdateDefaultWorkWeek")]
        public async Task<ActionResult> UpdateDefaultWorkWeek([FromBody]Dto.DefaultWorkweek defaultWorkweek)
        {
            if (defaultWorkweek == null)
            {
                return BadRequest();
            }
            if (defaultWorkweek.Id == null)
            {
                return BadRequest();
            }
            if (defaultWorkweek.Id == "")
            {
                return BadRequest();
            }
            Models.DefaultWorkweek defaultWorkweekModel = Mapper.Map<Models.DefaultWorkweek>(defaultWorkweek);
            if (await Repo.UpdateDefaultWorkWeek(defaultWorkweekModel))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("GetAllEmails")]
        public async Task<List<Dto.UserForEmail>> GetAllEmails()
        {
            if (User.FindFirst(ClaimTypes.Role).Value != Config.GetSection("Role:Human-Resources").Value)
            {
                Unauthorized();
            }
            List<string> Emails =  await Repo.GetAllMails();
            List<Dto.UserForEmail> userForEmails = new List<Dto.UserForEmail>();
            foreach (string email in Emails)
            {
                userForEmails.Add(new UserForEmail {Email=email });
            }
            return userForEmails;
        }
    }
}