﻿using System;
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
                BadRequest();
            }
            Models.User user = new Models.User { Id = UserId.Id };
            var userModel = Task.Run(() => Repo.Get(user));
            var ExceptionDaysIds = Task.Run(() => Repo.GetListOfExceptionDays(user));
            var LogsIds = Task.Run(() => Repo.GetListOfLogs(user));
            await Task.WhenAll(userModel, ExceptionDaysIds, LogsIds);
            if (userModel.Result == null)
            {
                BadRequest();
            }
            return new Dto.UserForGet { Id = userModel.Result.Id, Name = userModel.Result.Name, Email = userModel.Result.Email, LogIds = LogsIds.Result, ChangeHistory = userModel.Result.ChangeHistory, RoleId = userModel.Result.RoleId, DefaultWorkweekId = userModel.Result.DefaultWorkweekId, ExceptionWorkDayIds = ExceptionDaysIds.Result };
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
            var users = Task.Run(()=> Repo.GetAllConsultant());
            var workMonths = Task.Run(() => Repo.GetAllWorkMonths());
            await Task.WhenAll(users, workMonths);
            List<Dto.UserForGetHR> userDto = new List<Dto.UserForGetHR>();
            if (users.Result == null)
            {
                BadRequest();
            }
            foreach (Models.User user in users.Result)
            {
                Dto.UserForGetHR userForGetHR = Mapper.Map<Dto.UserForGetHR>(user);
                Dto.WorkMonth workMonth = Mapper.Map<Dto.WorkMonth>(workMonths.Result.Where(x => x.UserId == user.Id));
                workMonth.Salary = Repo.GetSalary(user);
                workMonth.TotalHours = Repo.GetTotalTime(user);
                userForGetHR.WorkMonth = workMonth;
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

            if (await Repo.UpdateWorkMonth(Mapper.Map<Models.WorkMonth>(workMonth)))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
