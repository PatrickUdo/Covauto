using Covauto.Shared.DTOs;
using Covauto.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace Covauto.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authRepository.RegisterAsync(registerDto);
            if (result.Succeeded) return Ok("User registered successfully.");
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        [EnableRateLimiting("LoginLimiter")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var (isSuccess, userId, userName, email) = await _authRepository.ValidateUserAsync(loginDto);

            if (!isSuccess)
            {
                return Unauthorized("Invalid credentials.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName!),
                new Claim(ClaimTypes.NameIdentifier, userId!),
                new Claim(ClaimTypes.Email, email!)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok("Login successful.");
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logged out.");
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            if (User?.Identity is not { IsAuthenticated: true })
            {
                return Unauthorized(new { Message = "User is not authenticated" });
            }

            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            return Ok(new
            {
                Email = email,
                UserId = userId,
                Username = username
            });
        }
    }
}