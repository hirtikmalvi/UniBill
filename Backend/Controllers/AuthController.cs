using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniBill.DTOs;
using UniBill.DTOs.Login;
using UniBill.DTOs.Registration;
using UniBill.Models;
using UniBill.Services.IServices;

namespace UniBill.Controllers
{
    [EnableCors("AllowedAngular")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0).SelectMany(kvp => kvp.Value.Errors.Select(err => err.ErrorMessage)).ToList();
                return BadRequest(CustomResult<User>.Fail("Registration Failed.", errors));
            }

            var result = await authService.RegisterUser(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0).SelectMany(kvp => kvp.Value.Errors.Select(err => err.ErrorMessage)).ToList();
                return BadRequest(CustomResult<LoginResponseDTO>.Fail("Login Failed.", errors));
            }

            var token = await authService.Login(request);

            if (!token.Success)
            {
                return BadRequest(token);
            }
            return Ok(token);
        }

        [HttpGet("validate")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            var response = CustomResult<string>.Ok("Token is valid", "Authentication Successful.");
            return Ok(response);
        }
    }
}
