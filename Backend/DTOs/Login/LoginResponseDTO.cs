namespace UniBill.DTOs.Login
{
    public class LoginResponseDTO
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public bool RequiresBusiness { get; set; }
    }
}
