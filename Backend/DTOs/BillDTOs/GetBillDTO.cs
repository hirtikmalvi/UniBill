using UniBill.DTOs.CustomerDTOs;
using UniBill.Models;

namespace UniBill.DTOs.BillDTOs
{
    public class GetBillDTO
    {
        public int BillId { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public GetCustomerDTO Customer { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? LabourCharges { get; set; }
        public List<GetBillItemDTO> BillItems { get; set; }
        public decimal SubTotal => BillItems?.Sum(bi => bi.Rate * bi.Quantity) ?? 0;
        public decimal DiscountAmount => SubTotal * (Discount ?? 0) / 100;
        public decimal TaxAmount => SubTotal * (Tax ?? 0) / 100;
        public decimal FinalTotal => SubTotal - DiscountAmount + TaxAmount + (LabourCharges ?? 0); 
    }
}
