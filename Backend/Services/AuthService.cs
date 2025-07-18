using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
    public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<CustomResult<RegistrationUserResponseDTO>> RegisterUser(RegisterUserDTO request)
        {
            if (await context.Users.AnyAsync(u => u.Email == request.Email.Trim()))
            {
                return CustomResult<RegistrationUserResponseDTO>.Fail("Email Already Exists.");
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
            if (user.Business == null)
            {
                return CustomResult<LoginResponseDTO>.Fail("Login Failed.", new List<string>
                {
                    "Create your business to login."
                });
            }
            var loginResponseDTO = new LoginResponseDTO
            {
                RefreshToken = string.Empty,
                AccessToken = GenerateJwtToken(user)
            };
            return CustomResult<LoginResponseDTO>.Ok(loginResponseDTO, "Login Successful.");
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("BusinessId", user.Business.BusinessId.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Key")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                expires: DateTime.Now.AddDays(3),
                signingCredentials: creds,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
