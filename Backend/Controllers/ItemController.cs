using System.Security.Claims;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniBill.DTOs;
using UniBill.DTOs.ItemDTOs;
using UniBill.Models;
using UniBill.Services.IServices;

namespace UniBill.Controllers
{
    [EnableCors("AllowedAngular")]
    [Route("api/items")]
    [Authorize]
    [ApiController]
    public class ItemController(IItemService itemService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0).SelectMany(kvp => kvp.Value.Errors).Select(err => err.ErrorMessage).ToList();
                
                return BadRequest(CustomResult<Item>.Fail("Could not create an Item.", errors));
            }

            request.BusinessId = Convert.ToInt32(User.FindFirst("BusinessId")?.Value);

            var result = await itemService.CreateItem(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await itemService.GetItems(userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var businessId = Convert.ToInt32(User.FindFirst("BusinessId")?.Value);

            var result = await itemService.GetItemById(id, businessId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var businessId = Convert.ToInt32(User.FindFirst("BusinessId")?.Value);

            var result = await itemService.DeleteItem(id, businessId);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemById(int id, [FromBody] PutItemRequestDTO itemToUpdate)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0).SelectMany(kvp => kvp.Value.Errors).Select(err => err.ErrorMessage).ToList();
                return BadRequest(CustomResult<Item>.Fail("Could not update an item.", errors));
            }

            itemToUpdate.BusinessId = Convert.ToInt32(User.FindFirst("BusinessId")?.Value);
            
            var result = await itemService.UpdateItem(id, itemToUpdate);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
