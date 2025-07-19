using System.Threading.Tasks;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using UniBill.Data;
using UniBill.DTOs;
using UniBill.DTOs.ItemDTOs;
using UniBill.Models;
using UniBill.Services.IServices;

namespace UniBill.Services
{ 
    public class ItemService(AppDbContext context) : IItemService
    {
        public async Task<CustomResult<GetItemDTO>> CreateItem(CreateItemDTO request)
        {
            if (!(await context.Businesses.AnyAsync(bt => bt.BusinessId == request.BusinessId)))
            {
                return CustomResult<GetItemDTO>.Fail("Item creation failed.", new List<string>
                {
                    $"Business for BusinessId: {request.BusinessId} does not exists."
                });
            }
            if (!(await context.Items.AnyAsync(i => i.BusinessId == request.BusinessId)))
            {
                return CustomResult<GetItemDTO>.Fail("Item creation failed.", new List<string>
                {
                    $"Business for BusinessId: {request.BusinessId} does not exists."
                });
            }
            if (!(await context.Units.AnyAsync(u => u.UnitId == request.UnitId)))
            {
                return CustomResult<GetItemDTO>.Fail("Item creation failed.", new List<string>
                {
                    $"Unit for UnitId: {request.UnitId} does not exists."
                });
            }
            if (!(await context.ItemTypes.AnyAsync(it => it.ItemTypeId == request.ItemTypeId)))
            {
                return CustomResult<GetItemDTO>.Fail("Item creation failed.", new List<string>
                {
                    $"ItemType for ItemTypeId: {request.ItemTypeId} does not exists."
                });
            }
            if (request.CategoryId != null && !(await context.Categories.AnyAsync(c => c.CategoryId == request.CategoryId)))
            {
                return CustomResult<GetItemDTO>.Fail("Item creation failed.", new List<string>
                {
                    $"Category for CategoryId: {request.CategoryId} does not exists."
                });
            }

            var item = new Item();

            item.ItemName = request.ItemName;
            item.ItemRate = request.ItemRate;
            item.UnitId = request.UnitId;
            item.ItemTypeId = request.ItemTypeId;
            item.BusinessId = (int)request.BusinessId!;
            item.CategoryId = request.CategoryId;

            var addedItem = await context.Items.AddAsync(item);
            await context.SaveChangesAsync();

            var itemToReturn = await context.Items.Where(i => i.ItemId == addedItem.Entity.ItemId).Select(i => new GetItemDTO
            {
                ItemId = i.ItemId,
                ItemName = i.ItemName,
                ItemRate = i.ItemRate,
                CategoryId = i.CategoryId,
                Category = i.Category.CategoryName,
                UnitId = i.UnitId,
                Unit = i.Unit.UnitName,
                UnitShortName = i.Unit.ShortUnitName,
                ItemTypeId = i.ItemTypeId,
                ItemType = i.ItemType.Name
            })
            .FirstOrDefaultAsync();

            return CustomResult<GetItemDTO>.Ok(itemToReturn!, "Item added successfully.");
        }

        public async Task<CustomResult<string>> DeleteItem(int itemId, int businessId)
        {
            var item = await context.Items.FirstOrDefaultAsync(i => i.ItemId == itemId);

            if (item == null)
            {
                return CustomResult<string>.Fail("Item could not be deleted.", new List<string>
                {
                    $"Item for ItemId: {itemId} does not exists."
                });
            }
            if (!(await context.Items.AnyAsync(i => i.ItemId == itemId && i.BusinessId == businessId)))
            {
                return CustomResult<string>.Fail("Item could not be deleted.", new List<string>
                {
                    $"You cannot delete the item of other business."
                });
            }
            context.Items.Remove(item);                               
            await context.SaveChangesAsync();

            return CustomResult<string>.Ok($"Item: {item.ItemId}", "Item deleted successfully.");
        }

        public async Task<CustomResult<GetItemDTO>> GetItemById(int itemId, int businessId)
        {
            if (!(await context.Items.AnyAsync(i => i.ItemId == itemId)))
            {
                return CustomResult<GetItemDTO>.Fail("Could not get Item.", new List<string>
                {
                    $"Item for ItemId: {itemId} does not exists."
                });
            }
            if (!(await context.Items.AnyAsync(i => i.ItemId == itemId && i.BusinessId == businessId)))
            {
                return CustomResult<GetItemDTO>.Fail("Could not get Item.", new List<string>
                {
                    "You are not allowed to view items of business other than yours."
                });
            }

            var item = await context.Items.Where(i => i.ItemId == itemId && i.BusinessId == businessId).Select(i => new GetItemDTO
            {
                ItemId = i.ItemId,
                ItemName = i.ItemName,
                ItemRate = i.ItemRate,
                CategoryId = i.CategoryId,
                Category = i.Category.CategoryName,
                UnitId = i.UnitId,
                Unit = i.Unit.UnitName,
                UnitShortName = i.Unit.ShortUnitName,
                ItemTypeId = i.ItemTypeId,
                ItemType = i.ItemType.Name
            })
            .FirstOrDefaultAsync();
            return CustomResult<GetItemDTO>.Ok(item!, "Item fetched successfully.");
        }

        public async Task<CustomResult<List<GetItemDTO>>> GetItems(int userId)
        {
            var user
                = await context.Users.Where(u => u.UserId == userId).Include(u => u.Business).FirstOrDefaultAsync();

            if (user == null)
            {
                return CustomResult<List<GetItemDTO>>.Fail("Failed to fetch Items.", new List<string>
                {
                    $"User with UserId: {userId} does not exists."
                });
            }

            var items = await context.Items.Where(i => i.BusinessId == user.Business.BusinessId).Select(i => new GetItemDTO
            {
                ItemId = i.ItemId,
                ItemName = i.ItemName,
                ItemRate = i.ItemRate,
                CategoryId = i.CategoryId,
                Category = i.Category.CategoryName,
                UnitId = i.UnitId,
                Unit = i.Unit.UnitName,
                UnitShortName = i.Unit.ShortUnitName,
                ItemTypeId = i.ItemTypeId,
                ItemType = i.ItemType.Name
            })
            .ToListAsync();

            if (items.Count == 0)
            {
                return CustomResult<List<GetItemDTO>>.Ok(items, $"No items found for Business with BusinessId: {user.Business.BusinessId}.");
            }

            return CustomResult<List<GetItemDTO>>.Ok(items, "Items fetched successfully.");
        }

        public async Task<CustomResult<GetItemDTO>> UpdateItem(int itemId, PutItemRequestDTO itemToUpdate)
        {
            if (itemId != itemToUpdate.ItemId)
            {
                return CustomResult<GetItemDTO>.Fail("Could not update item.", new List<string>
                {
                    "ItemId and ItemId in Item are not same."
                });
            }

            if (!(await context.Units.AnyAsync(u => u.UnitId == itemToUpdate.UnitId)))
            {
                return CustomResult<GetItemDTO>.Fail("Could not update item.", new List<string>
                {
                    $"Unit for UnitId: {itemToUpdate.UnitId} does not exists."
                });
            }
            if (!(await context.ItemTypes.AnyAsync(it => it.ItemTypeId == itemToUpdate.ItemTypeId)))
            {
                return CustomResult<GetItemDTO>.Fail("Item creation failed.", new List<string>
                {
                    $"ItemType for ItemTypeId: {itemToUpdate.ItemTypeId} does not exists."
                });
            }
            if (!(await context.Businesses.AnyAsync(bt => bt.BusinessId == itemToUpdate.BusinessId)))
            {
                return CustomResult<GetItemDTO>.Fail("Item creation failed.", new List<string>
                {
                    $"Business for BusinessId: {itemToUpdate.BusinessId} does not exists."
                });
            }
            if (itemToUpdate.CategoryId != null && !(await context.Categories.AnyAsync(c => c.CategoryId == itemToUpdate.CategoryId)))
            {
                return CustomResult<GetItemDTO>.Fail("Item creation failed.", new List<string>
                {
                    $"Category for CategoryId: {itemToUpdate.CategoryId} does not exists."
                });
            }
            
            var item = new Item()
            {
                ItemId = itemToUpdate.ItemId,
                ItemName = itemToUpdate.ItemName,
                ItemRate = itemToUpdate.ItemRate,
                CategoryId = itemToUpdate.CategoryId,
                UnitId = itemToUpdate.UnitId,
                ItemTypeId = itemToUpdate.ItemTypeId,
                BusinessId = (int)itemToUpdate.BusinessId!,
            };
            context.Items.Update(item);
            await context.SaveChangesAsync();

            var updatedItem = await context.Items.Where(i => i.ItemId == item.ItemId).Select(i => new GetItemDTO
            {
                ItemId = i.ItemId,
                ItemName = i.ItemName,
                ItemRate = i.ItemRate,
                CategoryId = i.CategoryId,
                Category = i.Category.CategoryName,
                UnitId = i.UnitId,
                Unit = i.Unit.UnitName,
                UnitShortName = i.Unit.ShortUnitName,
                ItemTypeId = i.ItemTypeId,
                ItemType = i.ItemType.Name
            })
            .FirstOrDefaultAsync();

            return CustomResult<GetItemDTO>.Ok(updatedItem!, "Item updated successfully.");
        }
    }
}
