using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UniBill.Validators;

namespace UniBill.DTOs.BillDTOs
{
    public class CreateBillDTO
    {
        [Required(ErrorMessage = "Date is Required.")]
        [ValidateBillDate]
        public DateTime Date {  get; set; }

        [Required(ErrorMessage = "Customer is Required.")]
        public int CustomerId { get; set; }

        [Range(0, 100,ErrorMessage = "Discount(%) cannot be negative and cannot exceed 100.")]
        [DefaultValue(0)]
        public decimal? Discount { get; set; }

        [Range(0, 100, ErrorMessage = "Tax(%) cannot be negative and cannot exceed 100.")]
        [DefaultValue(0)]
        public decimal? Tax { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = " Labour Charges cannot be negative.")]
        [DefaultValue(0)]
        public decimal? LabourCharges { get; set; }
        public List<CreateBillItemDTO> BillItems { get; set; }
    }
}
