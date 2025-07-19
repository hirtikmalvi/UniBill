using UniBill.DTOs;
using UniBill.DTOs.BusinessTypeDTOs;
using UniBill.DTOs.Categories;
using UniBill.DTOs.ItemTypeDTOs;
using UniBill.DTOs.UnitDTOs;

namespace UniBill.Services.IServices
{
    public interface ILookupService
    {
        Task<CustomResult<List<BusinessTypeDTO>>> GetBusinessTypes(); //
        Task<CustomResult<List<ItemTypeDTO>>> GetItemTypes();
        Task<CustomResult<List<CategoryDTO>>> GetCategories(); // 
        Task<CustomResult<List<UnitDTO>>> GetUnits();
        Task<CustomResult<List<UnitDTO>>> GetUnitsByBusiness(); //
        Task<CustomResult<List<ItemTypeDTO>>> GetItemTypesByBusiness(); //
        Task<CustomResult<List<CategoryDTO>>> GetCategoriesByItemType(int itemTypeId); //
    }
}
