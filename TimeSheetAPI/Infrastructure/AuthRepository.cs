using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetAPI.Models;
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
        
        public async Task<User> Login(string email, string password)
        {
            var user = await TimeSheetContext.User.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);

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
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            user.DefaultWorkweek = new DefaultWorkweek();

            user.Role = new Role { Name="Consultant" };
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
        //public async Task<ActionResult>
    }
}
