using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


namespace TimeSheetAPI.Infrastructure
{
    public class AuthRepository : IAuthRepository
    {
        private readonly TimeSheetContext TimeSheetContext;
        public AuthRepository(TimeSheetContext timeSheetContext)
        {
            TimeSheetContext = timeSheetContext;
        }
        
        public async Task<Models.User> Login(string email, string password)
        {
            Models.User user = await TimeSheetContext.User.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;

        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void CreatePasswordHash(string password, out Byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public async Task<Models.User> Register(Models.User user, string password)
        {
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            user.DefaultWorkweek = new Models.DefaultWorkweek();

            user.Role = new Models.Role { Name="Consultant" };
            await TimeSheetContext.AddAsync(user);
            await TimeSheetContext.SaveChangesAsync();
            return user;
        }
        public async Task<bool> UserExists(string email)
        {
            if (await TimeSheetContext.User.AnyAsync(x => x.Email == email))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CreateDefaultWorkweek(Dto.DefaultWorkweek defaultWorkweek)
        {
            var ModelDefaultWorkweek = new Models.DefaultWorkweek { Id = defaultWorkweek.Id, Monday = new Models.WorkDay { Id = defaultWorkweek.Monday.Id, Start = defaultWorkweek.Monday.Start, Stop = defaultWorkweek.Monday.Stop }, Tuesday = new Models.WorkDay { Id = defaultWorkweek.Tuesday.Id, Start = defaultWorkweek.Tuesday.Start, Stop= defaultWorkweek.Tuesday.Stop }, Wednesday= new Models.WorkDay { Id = defaultWorkweek.Wednesday.Id, Start = defaultWorkweek.Wednesday.Start, Stop = defaultWorkweek.Wednesday.Stop }, Thursday= new Models.WorkDay { Id = defaultWorkweek.Thursday.Id, Start = defaultWorkweek.Thursday.Start, Stop = defaultWorkweek.Thursday.Stop },Friday= new Models.WorkDay { Id = defaultWorkweek.Friday.Id, Start = defaultWorkweek.Friday.Start, Stop = defaultWorkweek.Friday.Stop },Saturday= new Models.WorkDay { Id = defaultWorkweek.Saturday.Id, Start = defaultWorkweek.Saturday.Start, Stop = defaultWorkweek.Saturday.Stop },Sunday= new Models.WorkDay { Id = defaultWorkweek.Sunday.Id, Start = defaultWorkweek.Sunday.Start, Stop = defaultWorkweek.Sunday.Stop}};
            try
            {
                TimeSheetContext.Update(ModelDefaultWorkweek);
                await TimeSheetContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ChangePassword(Dto.UserId userId, string password)
        {
            try
            {
                Models.User user = await TimeSheetContext.User.Where(x => x.Id == userId.Id).SingleAsync();
                if (user == null)
                {
                    return false;
                }
                else
                {
                    VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
                    TimeSheetContext.Update(user);
                }
                await TimeSheetContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}