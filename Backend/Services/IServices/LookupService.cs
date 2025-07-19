using Microsoft.EntityFrameworkCore;
using UniBill.Data;
using UniBill.DTOs;
using UniBill.DTOs.BusinessTypeDTOs;
using UniBill.DTOs.Categories;
using UniBill.DTOs.ItemTypeDTOs;
using UniBill.DTOs.UnitDTOs;
using UniBill.Models;

namespace UniBill.Services.IServices
{
    public class LookupService(IHttpContextAccessor httpContextAccessor, AppDbContext context) : ILookupService
    {
        public async Task<CustomResult<List<BusinessTypeDTO>>> GetBusinessTypes()
        {
            var businessTypes = await context.BusinessTypes.Select(bt => new BusinessTypeDTO
            {
                BusinessTypeId = bt.BusinessTypeId,
                BusinessTypeName = bt.Name
            }).ToListAsync();

            if (businessTypes.Count == 0)
            {
                return CustomResult<List<BusinessTypeDTO>>.Ok(businessTypes, "BusinessTypes fetched successfully and found No business types.");
            }
            return CustomResult<List<BusinessTypeDTO>>.Ok(businessTypes, "BusinessTypes fetched successfully.");
        }

        public async Task<CustomResult<List<CategoryDTO>>> GetCategories()
        {
            var categories = await context.Categories.Select(c => new CategoryDTO
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            }).ToListAsync();

            if (categories.Count == 0)
            {
                return CustomResult<List<CategoryDTO>>.Ok(categories, "Categories fetched successfully and found No Categories.");
            }
            return CustomResult<List<CategoryDTO>>.Ok(categories, "Categories fetched successfully.");
        }

        public async Task<CustomResult<List<CategoryDTO>>> GetCategoriesByItemType(int itemTypeId)
        {
            var categories = await context.Categories.Where(c => c.ItemTypeId == itemTypeId).Select(c => new CategoryDTO
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
            }).ToListAsync();

            if (categories.Count == 0)
            {
                return CustomResult<List<CategoryDTO>>.Ok(categories, "Category fetched successfully and found No Item Types.");
            }
            return CustomResult<List<CategoryDTO>>.Ok(categories, "Category fetched successfully.");
        }

        public async Task<CustomResult<List<ItemTypeDTO>>> GetItemTypes()
        {
            var itemTypes = await context.ItemTypes.Select(it => new ItemTypeDTO
            {
                ItemTypeId = it.ItemTypeId,
                ItemType = it.Name
            }).ToListAsync();

            if (itemTypes.Count == 0)
            {
                return CustomResult<List<ItemTypeDTO>>.Ok(itemTypes, "ItemTypes fetched successfully and found No Item Types.");
            }
            return CustomResult<List<ItemTypeDTO>>.Ok(itemTypes, "ItemTypes fetched successfully.");
        }

        public async Task<CustomResult<List<ItemTypeDTO>>> GetItemTypesByBusiness()
        {
            var user = httpContextAccessor.HttpContext?.User;
            var businessId = Convert.ToInt32(user.FindFirst("BusinessId")?.Value);

            var itemsTypes = await context.Businesses.Where(b => b.BusinessId == businessId).Select(b => context.AllowedItemTypes.Where(ait => ait.BusinessTypeId == b.BusinessTypeId).Include(ait => ait.ItemType).Select(ait => new ItemTypeDTO
            {
                ItemTypeId = ait.ItemType.ItemTypeId,
                ItemType = ait.ItemType.Name
            }).ToList()).FirstOrDefaultAsync();

            if (itemsTypes.Count == 0)
            {
                return CustomResult<List<ItemTypeDTO>>.Ok(itemsTypes, "ItemTypes fetched successfully and found No Units.");
            }
            return CustomResult<List<ItemTypeDTO>>.Ok(itemsTypes, "Units fetched successfully.");
        }

        public async Task<CustomResult<List<UnitDTO>>> GetUnits()
        {
            var units = await context.Units.Select(u => new UnitDTO
            {
                UnitId = u.UnitId,
                UnitName = u.UnitName,
                UnitShortName = u.ShortUnitName
            }).ToListAsync();

            if (units.Count == 0)
            {
                return CustomResult<List<UnitDTO>>.Ok(units, "Units fetched successfully and found No Units.");
            }
            return CustomResult<List<UnitDTO>>.Ok(units, "Units fetched successfully.");
        }

        public async Task<CustomResult<List<UnitDTO>>> GetUnitsByBusiness()
        {
            var user = httpContextAccessor.HttpContext?.User;
            var businessId = Convert.ToInt32(user.FindFirst("BusinessId")?.Value);

            var businessType = await context.Businesses.FirstOrDefaultAsync(b => b.BusinessId == businessId);

            var units = await context.Businesses.Where(b => b.BusinessId == businessId).Select(b => context.AllowedUnits.Where(au => au.BusinessTypeId == b.BusinessTypeId).Include(au => au.Unit).Select(au => new UnitDTO
            {
                UnitId = au.UnitId,
                UnitName = au.Unit.UnitName,
                UnitShortName = au.Unit.ShortUnitName
            }).ToList()
            ).FirstOrDefaultAsync();

            if (units.Count == 0)
            {
                return CustomResult<List<UnitDTO>>.Ok(units, "Units fetched successfully and found No Units.");
            }
            return CustomResult<List<UnitDTO>>.Ok(units, "Units fetched successfully.");
        }
    }
}
