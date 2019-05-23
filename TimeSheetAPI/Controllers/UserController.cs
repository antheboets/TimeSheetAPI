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
        public async Task<ActionResult<Dto.UserForGetHR>> Get([FromBody] Dto.UserForGet UserId)
        {
            if (UserId.Id == "")
            {
                return BadRequest();
            }
            if (UserId.Month == DateTime.MinValue)
            {
                UserId.Month = DateTime.Now;
            }
            Models.User user = new Models.User { Id = UserId.Id };
            /*  
            var userModel = Task.Run(() => Repo.Get(user));
            var ExceptionDaysIds = Task.Run(() => Repo.GetListOfExceptionDays(user));
            var LogsIds = Task.Run(() => Repo.GetListOfLogs(user));
            await Task.WhenAll(userModel, ExceptionDaysIds, LogsIds);
            */
            var userModel = await Repo.Get(user);
            userModel.Logs = await Repo.GetLogsOfUserMonth(userModel, UserId.Month);
            Dto.WorkMonth workMonthDto = null;
            var ExceptionDaysIds = await Repo.GetListOfExceptionDays(user);
            var LogsIds = Repo.LogsToIds(userModel.Logs);
            var workmMonth = await Repo.GetWorkMonths(userModel, UserId.Month);
            if (userModel == null)
            {
                return BadRequest();
            }

            if (workmMonth != null)
            {
                workMonthDto = new Dto.WorkMonth { Id = workmMonth.Id, Accepted = workmMonth.Accepted, Month = workmMonth.Month, UserId = workmMonth.UserId };
                workMonthDto.Salary = Repo.GetSalary(userModel);
                workMonthDto.TotalHours = Repo.GetTotalTime(userModel);
            }
            Dto.UserForGetHR userForGet = new Dto.UserForGetHR { Id = userModel.Id, Name = userModel.Name, ChangeHistory = userModel.ChangeHistory, Email = userModel.Email, DefaultWorkweekId = userModel.DefaultWorkweekId, RoleId = userModel.RoleId, ExceptionWorkDayIds = ExceptionDaysIds, LogIds = LogsIds, WorkMonth = workMonthDto };
            //Mapper.Map<Dto.UserForGet>(userModel.Result);
            if (userForGet == null)
            {
                return BadRequest();
            }
            if (userForGet.Id == "")
            {
                return BadRequest();
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
            return Ok(userForGet);
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
            Models.User user = new Models.User { Id = User.FindFirst(ClaimTypes.NameIdentifier).Value , Email = userForUpdate.Email, Name= userForUpdate.Name};
            if (await Repo.Update(user))
            {
                return Ok();
            }
            return BadRequest();
        }
        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<ActionResult<List<Dto.UserForGetFull>>> Test()
        {
            return Ok(Mapper.Map<List<Dto.UserForGetFull>>(await Repo.GetAll()));
        }
        //HR
        [HttpGet("GetConsultants")]
        public async Task<ActionResult<List<Dto.UserForGetHR>>> GetConsultants()
        {

            if (User.FindFirst(ClaimTypes.Role).Value != Config.GetSection("Role:Human-Resources").Value)
            {
                return Unauthorized();
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
                return BadRequest();
            }
            foreach (Models.User user in users)
            {

                Dto.UserForGetHR userForGetHR = new UserForGetHR { Id = user.Id, Email = user.Email, Name = user.Name, ChangeHistory = user.ChangeHistory };
                //Mapper.Map<Dto.UserForGetHR>(user);
                try
                {
                    Models.WorkMonth workMonthModel = workMonths.Where(x => x.UserId == user.Id).SingleOrDefault();
                    Dto.WorkMonth workMonth = new WorkMonth { Id = workMonthModel.Id, Month = workMonthModel.Month, Accepted = workMonthModel.Accepted, UserId = workMonthModel.UserId };
                    //Mapper.Map<Dto.WorkMonth>(workMonths.Result.Where(x => x.UserId == user.Id));
                    workMonth.Salary = Repo.GetSalary(user);
                    workMonth.TotalHours = Repo.GetTotalTime(user);
                    userForGetHR.WorkMonth = workMonth;

                }
                catch (Exception e)
                {
                    return BadRequest();
                }
                userDto.Add(userForGetHR);
            }
            return Ok(userDto);
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
                if (!workMonth.Accepted)
                {
                    Models.User user = await Repo.GetUserFromWorkMonth(workMonthModel);
                    if (user == null)
                    {
                        return BadRequest();
                    }
                    if (Repo.SendMail(workMonth.Body, user))
                    {
                        return Ok();
                    }
                }
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("GetDefaultWorkWeek")]
        public async Task<ActionResult<Dto.DefaultWorkweek>> GetDefaultWorkWeek([FromQuery] string Id)
        {
            if (Id == null)
            {
                return BadRequest();
            }
            if (Id == "")
            {
                return BadRequest();
            }
            Models.DefaultWorkweek defaultWorkweekModel = new Models.DefaultWorkweek { Id = Id };
            defaultWorkweekModel = await Repo.GetDefaultWorkweek(defaultWorkweekModel);
            if (defaultWorkweekModel == null)
            {
                return BadRequest();
            }
            return Ok(Mapper.Map<Dto.DefaultWorkweek>(defaultWorkweekModel));
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
        public async Task<ActionResult<List<Dto.UserForEmail>>> GetAllEmails()
        {
            if (User.FindFirst(ClaimTypes.Role).Value != Config.GetSection("Role:Human-Resources").Value)
            {
                return Unauthorized();
            }
            List<string> Emails = await Repo.GetAllMails();
            List<Dto.UserForEmail> userForEmails = new List<Dto.UserForEmail>();
            foreach (string email in Emails)
            {
                userForEmails.Add(new UserForEmail { Email = email });
            }
            return Ok(userForEmails);
        }
    }
}