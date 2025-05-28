using Covauto.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using Covauto.Application;

namespace Covauto.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto);
            if (result.Succeeded) return Ok("User registered successfully.");
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        [EnableRateLimiting("LoginLimiter")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var userByEmail = await _authService.FindUserByEmail(loginDto.Email);
            if (userByEmail != null && await _authService.IsLockedOut(userByEmail))
            {
                return StatusCode(StatusCodes.Status423Locked, "Try again later.");
            }

            var user = await _authService.ValidateUser(loginDto);

            if (user == null)
            {
                if (userByEmail != null && await _authService.IsLockedOut(userByEmail))
                {
                    return StatusCode(StatusCodes.Status423Locked, "Too many failed login attempts. Try again later.");
                }

                return Unauthorized("Invalid credentials.");
            }

            await _authService.ResetFailedAttempts(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
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
