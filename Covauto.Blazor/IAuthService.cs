using System.Threading.Tasks;

namespace Covauto.Blazor.Services
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticatedAsync();
        Task<(bool Success, string Message, string Username)> LoginAsync(string email, string password);
        Task<bool> LogoutAsync();
        Task<(bool Success, string Username, string Message)> GetUserInfoAsync();
    }
}
