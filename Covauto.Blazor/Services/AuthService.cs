using Covauto.Blazor.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Text.Json;
using Covauto.Shared.DTOs;

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

        public async Task<(bool Success, string Message)> RegisterAsync(string email, string password)
        {
            try
            {
                var registerData = new
                {
                    username = email,
                    email = email,
                    password = password
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/register");
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
                request.Content = new StringContent(
                    JsonSerializer.Serialize(registerData),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Registration successful");
                }
                else
                {
                    return (false, $"Registration failed: {content}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
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

        public async Task<UserInfoResult> GetUserInfoAsync()
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
                    var root = jsonDoc.RootElement;

                    var username = root.TryGetProperty("username", out var u) ? u.GetString() ?? string.Empty : string.Empty;
                    var email = root.TryGetProperty("email", out var e) ? e.GetString() ?? string.Empty : string.Empty;
                    var userId = root.TryGetProperty("userId", out var id) ? id.GetString() ?? string.Empty : string.Empty;

                    return new UserInfoResult
                    {
                        Success = true,
                        Username = username,
                        Email = email,
                        UserId = userId
                    };
                }
                else
                {
                    return new UserInfoResult
                    {
                        Success = false,
                        Message = $"Failed to get user info: {content}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new UserInfoResult
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}