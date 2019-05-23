using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

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
            if (user.Logs.Count > 0)
            {
                int curentMonth = (DateTime.Now.Year * 12) + DateTime.Now.Month;

                //List<Models.Log> logs = user.Logs.Where(x => ((x.Start.Year * 12) + x.Start.Month) == curentMonth && ((x.Stop.Year * 12) + x.Stop.Month) == curentMonth).ToList();
                int sec = 0;
                int min = 0;
                int hour = 0;
                foreach (Models.Log log in user.Logs)
                {
                    sec += log.Stop.Second - log.Start.Second;
                    min += log.Stop.Minute - log.Start.Minute;
                    hour += log.Stop.Hour - log.Start.Hour;
                }
                min += sec / 60;
                hour += min / 60;
                return hour + ":" + min + ":" + sec;
            }
            else
            {
                return "00:00:00";
            }
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
            try
            {
                Models.WorkMonth workMonthOld =  await TimeSheetContext.WorkMonth.Where(x => x.Id == workMonth.Id).SingleOrDefaultAsync();
                workMonth.Month = workMonthOld.Month;
                workMonth.UserId = workMonthOld.UserId;
                TimeSheetContext.Entry(workMonthOld).State = EntityState.Detached;
                workMonthOld = null;
                TimeSheetContext.Update(workMonth);
                await TimeSheetContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
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
                defaultWorkweek = await TimeSheetContext.DefaultWorkweek.Where(x => x.Id == defaultWorkweek.Id).Include(x => x.Monday).Include(x => x.Tuesday).Include(x => x.Wednesday).Include(x => x.Thursday).Include(x => x.Friday).Include(x => x.Saturday).Include(x => x.Sunday).SingleOrDefaultAsync();
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
                TimeSheetContext.Update(defaultWorkweek);
                await TimeSheetContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
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
        public async Task<Models.WorkMonth> GetWorkMonths(Models.User user, DateTime Time)
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
            int currentMonth = (Time.Year * 12) + Time.Month;
            Models.WorkMonth workMonth = null;
            try
            {
                workMonth = await TimeSheetContext.WorkMonth.Where(x => x.UserId == user.Id && x.Month == currentMonth).SingleOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
            if (workMonth == null)
            {
                return null;
            }
            return workMonth;
        }
        public async Task<List<Models.Log>> GetLogsOfUserMonth(User user, DateTime Time)
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
            int currentMonth = (Time.Year * 12) + Time.Month;
            List<Models.Log> logsOfMonth = null;
            try
            {
                logsOfMonth = await TimeSheetContext.Log.Where(x => x.UserId == user.Id && (x.Start.Year * 12) + x.Start.Month == currentMonth && (x.Stop.Year * 12) + x.Stop.Month == currentMonth).ToListAsync();
            }
            catch (Exception e)
            {
                return null;
            }
            return logsOfMonth;
        }
        public List<string> LogsToIds(ICollection<Models.Log> logs)
        {
            List<string> ids = new List<string>();
            foreach (Models.Log log in logs)
            {
                ids.Add(log.Id);
            }
            return ids;
        }
        public bool SendMail(string body, Models.User user)
        {
            MailMessage mailMessage = new MailMessage("ehbtimesheetapi@gmail.com", "ehbtimesheetapi@gmail.com"/*user.Email*/);
            mailMessage.Subject = "TimeSheet";
            mailMessage.Body = body;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential { UserName = "ehbtimesheetapi@gmail.com", Password = "k5QW4R9u5jtHFyQS" };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
            return true;
        }
        public async Task<Models.User> GetUserFromWorkMonth(Models.WorkMonth workMonth)
        {
            string UserId = await TimeSheetContext.WorkMonth.Where(x => x.Id == workMonth.UserId).Select(x => x.UserId).SingleOrDefaultAsync();
            return await TimeSheetContext.User.Where(x => x.Id == UserId).SingleOrDefaultAsync();
        }
    }
}