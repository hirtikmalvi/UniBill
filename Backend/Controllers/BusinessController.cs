using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniBill.DTOs;
using UniBill.DTOs.BusinessDTOs;
using UniBill.Services.IServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UniBill.Controllers
{
    [Route("api/business")]
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

        [HttpGet]
        public async Task<IActionResult> GetBusinessById(int businessId)
        {
            if (businessId <= 0)
            {
                return BadRequest(CustomResult<RegisterBusinessResponseDTO>.Fail("Could not get Business.", new List<string>
                {
                    $"Business does not exists for BusinessId: {businessId}"
                }));
            }
            var result = await businessService.GetBusiness(businessId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
