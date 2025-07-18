using UniBill.Models;

namespace UniBill.DTOs.Registration
{
    public class RegistrationUserResponseDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public UniBill.Models.Business Business { get; set; } 
    }
}
