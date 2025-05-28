using Covauto.Blazor.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Covauto.Blazor.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "api/auth/me");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(bool Success, string Message, string Username)> LoginAsync(string email, string password)
        {
            try
            {
                var loginData = new
                {
                    email = email,
                    password = password
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/login");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
                request.Content = new StringContent(
                    JsonSerializer.Serialize(loginData),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    string username = string.Empty;
                    try
                    {
                        var jsonDoc = JsonDocument.Parse(content);
                        if (jsonDoc.RootElement.TryGetProperty("username", out var usernameProp))
                        {
                            username = usernameProp.GetString() ?? string.Empty;
                        }
                    }
                    catch { /* Ignore parsing errors */ }

                    return (true, "Login successful", username);
                }
                else
                {
                    return (false, $"Login failed: {content}", string.Empty);
                }
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}", string.Empty);
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/logout");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(bool Success, string Username, string Message)> GetUserInfoAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "api/auth/me");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var jsonDoc = JsonDocument.Parse(content);
                    string username = string.Empty;

                    if (jsonDoc.RootElement.TryGetProperty("username", out var usernameProp))
                    {
                        username = usernameProp.GetString() ?? string.Empty;
                        return (true, username, string.Empty);
                    }
                    else
                    {
                        return (true, string.Empty, "Username not found in response");
                    }
                }
                else
                {
                    return (false, string.Empty, $"Failed to get user info: {content}");
                }
            }
            catch (Exception ex)
            {
                return (false, string.Empty, $"An error occurred: {ex.Message}");
            }
        }
    }
}