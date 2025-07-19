using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniBill.Services.IServices;

namespace UniBill.Controllers
{
    [EnableCors("AllowedAngular")]
    [Route("api/lookups")]
    [ApiController]
    public class LookupController(ILookupService lookupService) : ControllerBase
    {
        [HttpGet("business-types")]
        public async Task<IActionResult> GetBusinessTypes()
        {
            var result = await lookupService.GetBusinessTypes();
            return Ok(result);
        }

        [HttpGet("item-types")]
        [Authorize]
        public async Task<IActionResult> GetItemTypes()
        {
            var result = await lookupService.GetItemTypes();
            return Ok(result);
        }

        [HttpGet("categories")]
        [Authorize]
        public async Task<IActionResult> GetCategories()
        {
            var result = await lookupService.GetCategories();
            return Ok(result);
        }

        [HttpGet("units")]
        [Authorize]
        public async Task<IActionResult> GetUnits()
        {
            var result = await lookupService.GetUnits();
            return Ok(result);
        }

        [HttpGet("units/by-business")]
        [Authorize]
        public async Task<IActionResult> GetUnitsByBusiness()
        {
            var result = await lookupService.GetUnitsByBusiness();
            return Ok(result);
        }

        [HttpGet("item-types/by-business")]
        [Authorize]
        public async Task<IActionResult> GetItemTypesByBusiness()
        {
            var result = await lookupService.GetItemTypesByBusiness();
            return Ok(result);
        }

        [HttpGet("categories/by-item-type/{id}")]
        [Authorize]
        public async Task<IActionResult> GetCategoriesByItemType([FromRoute] int id)
        {
            var result = await lookupService.GetCategoriesByItemType(id);
            return Ok(result);
        }
    }
}
