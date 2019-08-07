using Microsoft.AspNetCore.Mvc;
using zwajapp.API.Data;
using System.Threading.Tasks;
using zwajapp.API.Models;
using zwajapp.API.Dtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace zwajapp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {

        userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
        if (await _repo.UserExists(userForRegisterDto.Username))
        return BadRequest(" User exist déja");
        var userToCreate=new User{
            Username=userForRegisterDto.Username
            
        };
        var CreatedUser= await _repo.Register(userToCreate,userForRegisterDto.Password);
        return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo= await _repo.Login(userForLoginDto.username.ToLower(),userForLoginDto.password);
            if(userFromRepo==null) return Unauthorized();
            var calims=new[]{
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds= new SigningCredentials(key,SecurityAlgorithms.HmacSha512);
            var tokenDescripror = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(calims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
                
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescripror);
            return Ok(new{
                token=tokenHandler.WriteToken(token)});
        }

        
    }
}