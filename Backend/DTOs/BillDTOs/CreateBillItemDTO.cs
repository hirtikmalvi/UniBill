using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UniBill.DTOs.BillDTOs;

public class CreateBillItemDTO
{
    [Required(ErrorMessage = "Item is required.")]
    public int ItemId { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, double.MaxValue, ErrorMessage = "Atleast 1 Quantity is required.")]
    public decimal Quantity { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Rate cannot be negative.")]
    [DefaultValue(0)]
    public decimal Rate { get; set; }
}
