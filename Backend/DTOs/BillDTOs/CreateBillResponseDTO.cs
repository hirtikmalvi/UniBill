using UniBill.Models;

namespace UniBill.DTOs.BillDTOs
{
    public class CreateBillResponseDTO
    {
        public int BillId { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public int TotalBillItems { get; set; }
    }
}
