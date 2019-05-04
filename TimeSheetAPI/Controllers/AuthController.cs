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

namespace TimeSheetAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository Repo;
        private readonly IConfiguration Config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            Repo = repo;
            Config = config;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Dto.UserForRegister user)
        {
            if (await Repo.UserExists(user.Email))
            {
                return BadRequest("Username already exists");
            }
            Models.User userToCreate = new Models.User { Email= user.Email.ToLower()};

            var createdUser = await Repo.Register(userToCreate, user.Password);

            return StatusCode(201);
        }
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
            
            /*
             * TODO remove this code if the code above does work
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id));
            claims.Add(new Claim(ClaimTypes.Email, userFromRepo.Email));
            claims.Add(new Claim(ClaimTypes.Role, userFromRepo.Role.Name));
            */

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new {token = tokenHandler.WriteToken(token)});
        }
        [HttpPost("NewPass")]
        public async Task<ActionResult> ChangePassword([FromBody] Dto.AuthNewPass authNewPass)
        {

            Dto.UserId userId = new UserId { Id = User.FindFirst(ClaimTypes.NameIdentifier).Value };

            await Repo.ChangePassword(userId, authNewPass.NewPassword);
            return Ok();
        }
    }
}