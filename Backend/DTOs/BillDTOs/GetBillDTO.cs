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
         public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public int? PaymentModeId { get; set; }
        public string? PaymentModeName { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? Notes { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public decimal SubTotal => BillItems?.Sum(bi => bi.Rate * bi.Quantity) ?? 0;
        public decimal DiscountAmount => SubTotal * (Discount ?? 0) / 100;
        public decimal TaxAmount => SubTotal * (Tax ?? 0) / 100;
        public decimal FinalTotal => SubTotal - DiscountAmount + TaxAmount + (LabourCharges ?? 0); 
    }
}
