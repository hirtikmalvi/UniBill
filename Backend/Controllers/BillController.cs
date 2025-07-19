using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniBill.DTOs;
using UniBill.DTOs.BillDTOs;
using UniBill.Services.IServices;

namespace UniBill.Controllers
{
    [EnableCors("AllowedAngular")]
    [Route("api/bills")]
    [Authorize]
    [ApiController]
    public class BillController(IBillService billService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateBill([FromBody] CreateBillDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0).SelectMany(kvp => kvp.Value.Errors).Select(err => err.ErrorMessage).ToList();
                
                return BadRequest(CustomResult<CreateBillResponseDTO>.Fail("Could not create bill.", errors));
            }
            var result = await billService.CreateBill(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBills()
        {
            var result = await billService.GetAllBills();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillById([FromRoute] int id)
        {
            var result = await billService.GetBillById(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
