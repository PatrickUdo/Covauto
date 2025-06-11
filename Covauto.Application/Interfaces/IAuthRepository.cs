using Covauto.Shared.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Covauto.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
        Task<(bool IsSuccess, string? UserId, string? UserName, string? Email)> ValidateUserAsync(LoginDto loginDto);
    }
}