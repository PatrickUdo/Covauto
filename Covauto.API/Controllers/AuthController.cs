using Covauto.Application.DTOs;
using Covauto.Application;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

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
                return StatusCode(StatusCodes.Status423Locked, "Account is locked out due to too many failed login attempts. Please try again later.");
            }

            var user = await _authService.ValidateUser(loginDto);

            if (user == null)
            {
                if (userByEmail != null && await _authService.IsLockedOut(userByEmail))
                {
                    return StatusCode(StatusCodes.Status423Locked, "Account is now locked out due to too many failed login attempts. Please try again later.");
                }

                return Unauthorized("Invalid credentials.");
            }

            await _authService.ResetFailedAttempts(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
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
                return Unauthorized(new { Message = "User is NOT authenticated" });
            }
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(new
            {
                Username = User.Identity.Name,
                Claims = claims
            });
        }
    }
}
