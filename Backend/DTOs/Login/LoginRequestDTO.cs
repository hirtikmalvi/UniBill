using System.ComponentModel.DataAnnotations;

namespace UniBill.DTOs.Login
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Valid Email address is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
