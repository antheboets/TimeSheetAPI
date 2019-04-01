using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TimeSheetAPI.Infrastructure
{
    public interface IAuthRepository
    {
        Task<Models.User> Register(Models.User user, string password);
        Task<Models.User> Login(string email, string password);
        Task<bool> UserExists(string email);
        Task<bool> CreateDefaultWorkweek(Dto.DefaultWorkweek defaultWorkweek);
        Task<bool> ChangePassword(Dto.UserId userId, string password);
    }
}
