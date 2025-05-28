namespace Covauto.Shared.DTOs
{
    public class UserInfoResult
    {
        public bool Success { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}