using UniBill.DTOs.ItemDTOs;
using UniBill.Models;

namespace UniBill.DTOs.BillDTOs
{
    public class GetBillItemDTO
    {
        public int BillItemId { get; set; }
        public GetItemDTO Item { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal? Total { get; set; }
    }
}
