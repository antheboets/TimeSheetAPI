using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace TimeSheetAPI.Infrastructure
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string email, string password);
        Task<bool> UserExists(string email);
        //DefaultWorkweek CreateDefaultWorkweek();
        //Task<IActionResult> ChangePassword(string password);
    }
}
