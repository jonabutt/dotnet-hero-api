using HeroAPI.Helpers.Auth;
using HeroAPI.Helpers.JWT;
using HeroAPI.Models;
using HeroAPI.Services.UserServices;
using HeroAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HeroAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<HeroController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(DataContext context, ILogger<HeroController> logger, IConfiguration configuration,IUserService userService)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _userService = userService;
        }

        // GET: api/v1/Auth/GetMe
        [HttpGet("GetMe"), Authorize]
        public ActionResult<string> GetMe()
        {
            string username = _userService.GetUserName();
            return Ok(username);
        }

        // POST: api/v1/Auth/register
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterViewModel registerDetails)
        {
            // TODO: validation using DataAnnotations
            PasswordHelper.CreatePasswordHash(registerDetails.Password, out byte[] salt, out byte[] passwordHash);
            await _context.Users.AddAsync(new User()
            {
                CreatedAt = DateTime.UtcNow,
                PasswordHash = passwordHash,
                PasswordSalt = salt,
                Username = registerDetails.Username
            });
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/v1/Auth
        [HttpPost]
        public async Task<ActionResult<string>> Login(LoginViewModel loginDetails)
        {
            // TODO: validation using DataAnnotations
            User? user = await _context.Users
                .Where(u => u.Username == loginDetails.Username)
                .FirstOrDefaultAsync();
            if (user == null)
                return NotFound("User not found.");

            if (!PasswordHelper.PasswordIsValid(loginDetails.Password, user.PasswordSalt, user.PasswordHash))
            {
                return BadRequest("Invalid password.");
            }
            string token = JWTHelper.CreateToken(user, _configuration.GetSection("AppSettings:JWTToken").Value);
            return Ok(token);
        }
    }
}
