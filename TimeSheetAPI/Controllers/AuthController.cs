using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Dto;
using TimeSheetAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace TimeSheetAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository Repo;
        private readonly IConfiguration Config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this.Repo = repo;
            this.Config = config;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Dto.UserForRegister user)
        {
            if (await Repo.UserExists(user.Email))
            {
                return BadRequest("Username already exists");
            }
            string roleId = "";
            if (Config.GetSection("Role:Consultant:Name").Value == user.Role)
            {
                roleId = Config.GetSection("Role:Consultant:Id").Value;
            }
            if (Config.GetSection("Role:Manager:Name").Value == user.Role)
            {
                roleId = Config.GetSection("Role:Consultant:Id").Value;
            }
            if (Config.GetSection("Role:Human-Resources:Name").Value == user.Role)
            {
                roleId = Config.GetSection("Role:Consultant:Id").Value;
            }
            if (roleId == "")
            {
                return BadRequest();
            }
            Models.User userToCreate = new Models.User { Email = user.Email.ToLower(), HourlyRate = user.Sal, RoleId = roleId };

            var createdUser = await Repo.Register(userToCreate, user.Password);

            return StatusCode(201);
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Dto.UserForLogin user)
        {
            var userFromRepo = await Repo.Login(user.Email.ToLower(), user.Password);
            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id),
                new Claim(ClaimTypes.Email, userFromRepo.Email),
                new Claim(ClaimTypes.Role, userFromRepo.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(365),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { token = tokenHandler.WriteToken(token) });
        }
        [HttpPost("NewPass")]
        public async Task<ActionResult<Dto.Success>> ChangePassword([FromBody] Dto.AuthNewPass authNewPass)
        {
            Dto.UserId userId = new UserId { Id = User.FindFirst(ClaimTypes.NameIdentifier).Value };

            if (authNewPass == null)
            {
                return BadRequest(new Success { SuccessState = false });
            }
            if (await Repo.ChangePassword(userId, authNewPass.NewPassword))
            {
                return Ok(new Success { SuccessState = true });
            }
            return Ok(new Success { SuccessState = true });
        }
    }
}
