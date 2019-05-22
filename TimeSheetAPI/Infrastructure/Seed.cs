using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetAPI.Models;
using Newtonsoft.Json;

namespace TimeSheetAPI.Infrastructure
{
    public class Seed
    {
        private readonly TimeSheetContext timeSheetContext;

        public Seed(TimeSheetContext timeSheetContext)
        {
            this.timeSheetContext = timeSheetContext;
        }
        public void SeedCompany()
        {
            var companyData = System.IO.File.ReadAllText("./SeedData/CompanySeed.json");
            List<Company> companies = JsonConvert.DeserializeObject<List<Company>>(companyData);
            foreach (var company in companies)
            {
                timeSheetContext.Company.Add(company);
            }
            timeSheetContext.SaveChanges();
        }
        public void SeedRole()
        {
            var roleData = System.IO.File.ReadAllText("./SeedData/RoleSeed.json");
            List<Role> roles = JsonConvert.DeserializeObject<List<Role>>(roleData);
            foreach (var role in roles)
            {
                timeSheetContext.Role.Add(role);
            }
            timeSheetContext.SaveChanges();
        }
        public void SeedUser()
        {
            var userData = System.IO.File.ReadAllText("./SeedData/UserSeed.json");
            List<User> users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                byte[] passwordHash;
                byte[] passwordSalt;
                CreatePasswordHash("123", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                

                //user.DefaultWorkweek = new DefaultWorkweek();
                timeSheetContext.User.Add(user);
            }
            timeSheetContext.SaveChanges();
        }
        public void SeedWorkMonth()
        {
            var WorkMonthData = System.IO.File.ReadAllText("./SeedData/WorkMonthSeed.json");
            List<WorkMonth> workMonths = JsonConvert.DeserializeObject<List<WorkMonth>>(WorkMonthData);
            foreach (var workMonth in workMonths)
            {
                timeSheetContext.WorkMonth.Add(workMonth);
            }
            timeSheetContext.SaveChanges();
        }
        private void CreatePasswordHash(string password, out Byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public void SeedDefaultWorkweek()
        {
            var DefaultWorkweekData = System.IO.File.ReadAllText("./SeedData/DefaultWorkweekSeed.json");
            List<DefaultWorkweek> defaultWorkweeklist = JsonConvert.DeserializeObject<List<DefaultWorkweek>>(DefaultWorkweekData);
            foreach (var defaultWorkweek in defaultWorkweeklist)
            {
                timeSheetContext.DefaultWorkweek.Add(defaultWorkweek);
            }
            timeSheetContext.SaveChanges();
        }
        public void SeedProject()
        {
            var ProjectData = System.IO.File.ReadAllText("./SeedData/ProjectSeed.json");
            List<Project> Projects = JsonConvert.DeserializeObject<List<Project>>(ProjectData);
            foreach (var project in Projects)
            {
                timeSheetContext.Project.Add(project);
            }
            timeSheetContext.SaveChanges();
        }
        public void SeedProjectUsers()
        {
            var ProjectUserData = System.IO.File.ReadAllText("./SeedData/ProjectUserSeed.json");
            List<ProjectUser> ProjectUsers = JsonConvert.DeserializeObject<List<ProjectUser>>(ProjectUserData);
            foreach (var projectUser in ProjectUsers)
            {
                timeSheetContext.ProjectUser.Add(projectUser);
            }
            timeSheetContext.SaveChanges();
        }
        public void SeedActivity()
        {
            var ActivityData = System.IO.File.ReadAllText("./SeedData/ActivitySeed.json");
            List<Activity> activities  = JsonConvert.DeserializeObject<List<Activity>>(ActivityData);
            foreach (var activity in activities)
            {
                timeSheetContext.Activity.Add(activity);
            }
            timeSheetContext.SaveChanges();
        }
        public void SeedLog()
        {
            var LogData = System.IO.File.ReadAllText("./SeedData/LogSeed.json");
            List<Log> Logs = JsonConvert.DeserializeObject<List<Log>>(LogData);
            foreach (var log in Logs)
            {
                timeSheetContext.Log.Add(log);
            }
            timeSheetContext.SaveChanges();
        }
        public void SeedAll()
        {
            SeedCompany();
            SeedRole();
            SeedDefaultWorkweek();
            SeedUser();
            SeedWorkMonth();
            SeedProject();
            SeedProjectUsers();
            SeedActivity();
            SeedLog();
        }
    }
}
