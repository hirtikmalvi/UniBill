using System.ComponentModel.DataAnnotations;

namespace UniBill.DTOs.Registration
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Valid Email address is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@#$]).{6,15}$", ErrorMessage = "Password must be 6-15 characters, include a letter, number, and special character (@, #, $).")]
        public string Password { get; set; }
    }
}
