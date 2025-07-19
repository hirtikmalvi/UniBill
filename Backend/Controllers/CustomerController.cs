using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniBill.DTOs;
using UniBill.DTOs.CustomerDTOs;
using UniBill.Services.IServices;

namespace UniBill.Controllers
{
    [EnableCors("AllowedAngular")]
    [Route("api/customers")]
    [Authorize]
    [ApiController]
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0).SelectMany(kvp => kvp.Value.Errors).Select(err => err.ErrorMessage).ToList();

                return BadRequest(CustomResult<GetCustomerDTO>.Fail("Could not create Customer.", errors));
            }
            var result = await customerService.CreateCustomer(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var result = await customerService.GetAllCustomers();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] int id)
        {
            var result = await customerService.GetCustomerById(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("by-mobile-no/{mobileNumber}")]
        public async Task<IActionResult> GetCustomerByMobileNumber([FromRoute] string mobileNumber)
        {
            var result = await customerService.GetCustomerByMobileNumber(mobileNumber);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
