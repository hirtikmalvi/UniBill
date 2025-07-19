using UniBill.DTOs;
using UniBill.DTOs.BillDTOs;
using UniBill.Models;

namespace UniBill.Services.IServices
{
    //POST    /api/bills
    //GET     /api/bills
    //GET     /api/bills/{id}
    public interface IBillService
    {
        Task<CustomResult<CreateBillResponseDTO>> CreateBill(CreateBillDTO request);
        Task<CustomResult<List<GetAllBillResponseDTO>>> GetAllBills();
        Task<CustomResult<GetBillDTO>> GetBillById(int billId);
    }
}
