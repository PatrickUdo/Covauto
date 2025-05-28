using System.Threading.Tasks;
using Covauto.Shared.DTOs;

namespace Covauto.Blazor.Interfaces
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticatedAsync();
        Task<(bool Success, string Message, string Username)> LoginAsync(string email, string password);
        Task<bool> LogoutAsync();
        Task<UserInfoResult> GetUserInfoAsync();
    }
}