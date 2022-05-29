using HeroAPI.Helpers.Auth;
using HeroAPI.Models;
using HeroAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeroAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<HeroController> _logger;
        public AuthController(DataContext context, ILogger<HeroController> logger)
        {
            _context = context;
            _logger = logger;
        }


        // POST: api/v1/Hero/register
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterViewModel registerDetails)
        {
            // TODO: validation using DataAnnotations
            PasswordHelper.CreatePasswordHash(registerDetails.Password,out byte[] salt,out byte[] passwordHash);
            await _context.Users.AddAsync(new User()
            {
                CreatedAt = DateTime.UtcNow,
                PasswordHash = salt,
                PasswordSalt = passwordHash,
                Username = registerDetails.Username
            });
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
