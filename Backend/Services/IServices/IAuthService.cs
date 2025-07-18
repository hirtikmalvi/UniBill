using UniBill.DTOs;
using UniBill.DTOs.Login;
using UniBill.DTOs.Registration;
using UniBill.Models;

namespace UniBill.Services.IServices
{
    public interface IAuthService
    {
        Task<CustomResult<RegistrationUserResponseDTO>> RegisterUser(RegisterUserDTO request);
        Task<CustomResult<LoginResponseDTO>> Login(LoginRequestDTO request);
    }
}
