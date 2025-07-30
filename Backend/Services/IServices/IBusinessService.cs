using UniBill.DTOs;
using UniBill.DTOs.BusinessDTOs;
using UniBill.Models;

namespace UniBill.Services.IServices
{
    public interface IBusinessService
    {
        Task<CustomResult<RegisterBusinessResponseDTO>> RegisterBusiness(RegisterBusinessDTO request);
        Task<CustomResult<GetBusinessDTO>> GetBusiness();
        Task<CustomResult<bool>> DoesUserHasBusiness();
    }
}
