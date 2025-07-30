using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniBill.DTOs;
using UniBill.DTOs.BusinessDTOs;
using UniBill.Services.IServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UniBill.Controllers
{
    [EnableCors("AllowedAngular")]
    [Route("api/business")]
    [Authorize]
    [ApiController]
    public class BusinessController(IBusinessService businessService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterBusinessDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0).SelectMany(kvp => kvp.Value.Errors.Select(err => err.ErrorMessage)).ToList();
                return BadRequest(CustomResult<RegisterBusinessResponseDTO>.Fail("Business Registration Failed.", errors));
            }
            var result = await businessService.RegisterBusiness(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetBusinessById()
        {
            var result = await businessService.GetBusiness();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("has-business")]
        public async Task<IActionResult> DoesUserHasBusiness()
        {
            var result = await businessService.DoesUserHasBusiness();
            if (!result.Success) {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
