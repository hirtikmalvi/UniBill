using UniBill.DTOs;
using UniBill.DTOs.ItemDTOs;
using UniBill.Models;

namespace UniBill.Services.IServices
{
    public interface IItemService
    {
        Task<CustomResult<GetItemDTO>> CreateItem(CreateItemDTO request);
        Task<CustomResult<List<GetItemDTO>>> GetItems(int userId);
        Task<CustomResult<GetItemDTO>> GetItemById(int itemId, int businessId);
        Task<CustomResult<GetItemDTO>> UpdateItem(int itemId, PutItemRequestDTO itemToUpdate);
        Task<CustomResult<string>> DeleteItem(int itemId, int businessId);
    }
}
