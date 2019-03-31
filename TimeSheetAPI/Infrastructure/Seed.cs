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
                

                user.DefaultWorkweek = new DefaultWorkweek();
                timeSheetContext.User.Add(user);
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
        public void SeedAll()
        {
            SeedCompany();
            SeedRole();
            SeedUser();
        }
    }
}
