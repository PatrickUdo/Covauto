using Covauto.Application.Interfaces;
using Covauto.Domain.Entities;
using Covauto.Shared.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Covauto.Domain
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };
            return await _userManager.CreateAsync(user, registerDto.Password);
        }

        public async Task<(bool IsSuccess, string? UserId, string? UserName, string? Email)> ValidateUserAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return (false, null, null, null);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return (true, user.Id, user.UserName, user.Email);
            }

            return (false, null, null, null);
        }
    }
}