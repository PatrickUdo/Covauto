using Microsoft.AspNetCore.Identity;
using Covauto.Domain.Entities;
using Covauto.Application.DTOs;

namespace Covauto.Application
{
    public class AuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };
            return await _userManager.CreateAsync(user, registerDto.Password);
        }

        public async Task<AppUser?> ValidateUser(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

            return result.Succeeded ? user : null;
        }

        public async Task<AppUser?> FindUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> IsLockedOut(AppUser user)
        {
            return await _userManager.IsLockedOutAsync(user);
        }

        public async Task RegisterFailedAttempt(AppUser user)
        {
            await _userManager.AccessFailedAsync(user);
        }

        public async Task ResetFailedAttempts(AppUser user)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
        }
    }
}
