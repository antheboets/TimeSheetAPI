﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Models;
using Microsoft.Extensions.Configuration;

namespace TimeSheetAPI.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly TimeSheetContext TimeSheetContext;
        private readonly IConfiguration Config;

        public UserRepository(TimeSheetContext TimeSheetContext, IConfiguration Config)
        {
            this.TimeSheetContext = TimeSheetContext;
            this.Config = Config;
        }
        public async Task<bool> Delete(Models.User user)
        {
            if (user == null)
            {
                return false;
            }
            if (user.Id == null)
            {
                return false;
            }
            if (user.Id == "")
            {
                return false;
            }
            TimeSheetContext.User.Remove(user);
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }
        public async Task<Models.User> Get(Models.User user)
        {
            if (user == null)
            {
                return null;
            }
            if (user.Id == null)
            {
                return null;
            }
            if (user.Id == "")
            {
                return null;
            }
            try
            {
                user = await TimeSheetContext.User.Where(x => x.Id == user.Id).SingleOrDefaultAsync();
            }
            catch (InvalidOperationException e)
            {
                return null;
            }
            if (user == null)
            {
                return null;
            }
            return user;
        }
        public async Task<List<Models.User>> GetAll()
        {
            return await TimeSheetContext.User.Include(x => x.DefaultWorkweek).Include(x => x.Logs).Include(x => x.Role).Include(x => x.ExceptionWorkDays).ToListAsync();
        }
        public async Task<List<Models.User>> GetAllConsultant()
        {
            return await TimeSheetContext.User.Where(x=> x.RoleId == Config.GetSection("Role:Consultant:Id").Value).Include(x => x.Logs).ToListAsync();
        }
        public async Task<List<WorkMonth>> GetAllWorkMonths()
        {
            int cunrentMont = (DateTime.Now.Year * 12) + DateTime.Now.Month;
            return await TimeSheetContext.WorkMonth.Where(x => x.Month == cunrentMont).ToListAsync();
        }
        public async Task<List<string>> GetListOfExceptionDays(User user)
        {
            if (user == null)
            {
                return null;
            }
            if (user.Id == null)
            {
                return null;
            }
            if (user.Id == "")
            {
                return null;
            }
            List<Models.WorkDayException> workDayExceptions = await TimeSheetContext.WorkDayException.Where(x => x.UserId == user.Id).ToListAsync();
            List<string> workDayExceptionsIds = new List<string>();
            foreach (Models.WorkDayException workDayException  in workDayExceptions)
            {
                workDayExceptionsIds.Add(workDayException.Id);
            }
            return workDayExceptionsIds;
        }
        public async Task<List<string>> GetListOfLogs(Models.User user)
        {
            if (user == null)
            {
                return null;
            }
            if (user.Id == null)
            {
                return null;
            }
            if (user.Id == "")
            {
                return null;
            }
            List<Models.Log> logs = await TimeSheetContext.Log.Where(x => x.UserId == user.Id).ToListAsync();
            List<string> logsId = new List<string>();
            foreach (Log User in logs)
            {
                logsId.Add(User.Id);
            }
            return logsId;
        }
        public string GetSalary(Models.User user)
        {
            if (user == null)
            {
                return "";
            }
            if (user.Id == null)
            {
                return "";
            }
            if (user.Id == "")
            {
                return "";
            }
            return "€1,00";
        }
        public string GetTotalTime(Models.User user)
        {
            if (user == null)
            {
                return "";
            }
            if (user.Id == null)
            {
                return "";
            }
            if (user.Id == "")
            {
                return "";
            }
            int curentMonth = (DateTime.Now.Year * 12) + DateTime.Now.Month;
            List<Models.Log> logs = user.Logs.Where(x => ((x.Start.Year * 12) + x.Start.Month) == curentMonth && ((x.Stop.Year * 12) + x.Stop.Month) == curentMonth).ToList();
            int sec = 0;
            int min = 0;
            int hour = 0;
            foreach (Models.Log log in logs)
            {
                sec += log.Stop.Second - log.Start.Second;
                min += log.Stop.Minute - log.Start.Minute;
                hour += log.Stop.Hour - log.Start.Hour;
            }
            min += sec / 60;
            hour += min / 60;
            return hour + ":" + min + ":" +sec;
        }
        public async Task<bool> Update(Models.User user)
        {
            if (user == null)
            {
                return false;
            }
            if (user.Id == null)
            {
                return false;
            }
            if (user.Id == "")
            {
                return false;
            }
            TimeSheetContext.Update(user);
            await TimeSheetContext.SaveChangesAsync();
            //.User.Include(x => x.Logs).SingleAsync(x => x.Id == input.Id);
            return true;
        }
        public async Task<bool> UpdateWorkMonth(Models.WorkMonth workMonth)
        {
            if (workMonth == null)
            {
                return false;
            }
            if (workMonth.Id == null)
            {
                return false;
            }
            if (workMonth.Id == "")
            {
                return false;
            }
            TimeSheetContext.Update(workMonth);
            await TimeSheetContext.SaveChangesAsync();
            //.User.Include(x => x.Logs).SingleAsync(x => x.Id == input.Id);
            return true;
        }
        public async Task<DefaultWorkweek> GetDefaultWorkweek(Models.DefaultWorkweek defaultWorkweek)
        {
            if (defaultWorkweek == null)
            {
                return null;
            }
            if (defaultWorkweek.Id == null)
            {
                return null;
            }
            if (defaultWorkweek.Id == "")
            {
                return null;
            }
            try
            {
                defaultWorkweek = await TimeSheetContext.DefaultWorkweek.Where(x => x.Id == defaultWorkweek.Id).SingleOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
            return defaultWorkweek;
        }
        public async Task<bool> UpdateDefaultWorkWeek(Models.DefaultWorkweek defaultWorkweek)
        {
            if (defaultWorkweek == null)
            {
                return false;
            }
            if (defaultWorkweek.Id == null)
            {
                return false;
            }
            if (defaultWorkweek.Id == "")
            {
                return false;
            }
            try
            {
                await TimeSheetContext.DefaultWorkweek.Where(x => x.Id == defaultWorkweek.Id).SingleOrDefaultAsync();
                TimeSheetContext.DefaultWorkweek.Update(defaultWorkweek);
            }
            catch (Exception e)
            {
                return false;
            }
            await TimeSheetContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<string>> GetAllMails()
        {
            List<string> emails = new List<string>();
            List<Models.User> users = await TimeSheetContext.User.Where(x => x.RoleId == Config.GetSection("Role:Consultant:Id").Value).ToListAsync();
            foreach (Models.User user in users)
            {
                emails.Add(user.Email);
            }
            return emails;
        }
    }
}
