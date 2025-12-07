using UniBill.DTOs.CustomerDTOs;
using UniBill.Models;

namespace UniBill.DTOs.BillDTOs
{
    public class GetAllBillResponseDTO
    {
        public int BillId { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public string CustomerMobileNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal LabourCharges { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount => SubTotal * Discount / 100;
        public decimal TaxAmount => SubTotal * Tax / 100;
        public decimal FinalTotal => SubTotal - DiscountAmount + TaxAmount + LabourCharges;
        public int? BillStatusId { get; set; }
        public string? BillStatus { get; set; }
        public int? PaymentModeId { get; set; }
        public string? PaymentMode { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}