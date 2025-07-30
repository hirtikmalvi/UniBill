using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniBill.Data;
using UniBill.DTOs;
using UniBill.DTOs.Login;
using UniBill.DTOs.Registration;
using UniBill.Models;
using UniBill.Services.IServices;

namespace UniBill.Services
{
    public class AuthService(AppDbContext context, ITokenService tokenService) : IAuthService
    {
        public async Task<CustomResult<RegistrationUserResponseDTO>> RegisterUser(RegisterUserDTO request)
        {
            if (await context.Users.AnyAsync(u => u.Email == request.Email.Trim()))
            {
                return CustomResult<RegistrationUserResponseDTO>.Fail("Registration Failed.", [
                    "Email Already Exists."
                ]);
            }
            var user = new User();
            user.Email = request.Email;
            user.Password = new PasswordHasher<User>().HashPassword(user, request.Password);
            await context.AddAsync(user);
            await context.SaveChangesAsync();

            var registrationUserRes = new RegistrationUserResponseDTO
            {
                UserId = user.UserId, 
                Email = user.Email, 
                Business = user.Business
            };

            return CustomResult<RegistrationUserResponseDTO>.Ok(registrationUserRes, "User registered successfully.");
        }
        public async Task<CustomResult<LoginResponseDTO>> Login(LoginRequestDTO request)
        {
            var user = await context.Users.Include(u => u.Business).FirstOrDefaultAsync(u => u.Email == request.Email.Trim());
            if (user == null)
            {
                return CustomResult<LoginResponseDTO>.Fail("Login Failed.", new List<string>
                {
                    "Invalid Credentials."
                });
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Failed)
            {
                return CustomResult<LoginResponseDTO>.Fail("Login Failed.", new List<string>
                {
                    "Invalid Credentials."
                });
            }
            var loginResponseDTO = new LoginResponseDTO
            {
                RefreshToken = string.Empty,
                AccessToken = tokenService.GenerateJwtToken(user),
                RequiresBusiness = user.Business == null
            };
            string message = user.Business == null
                            ? "Login successful, but no business found. Please register your business."
                            : "Login successful.";
            return CustomResult<LoginResponseDTO>.Ok(loginResponseDTO, message);
        }
    }
}
