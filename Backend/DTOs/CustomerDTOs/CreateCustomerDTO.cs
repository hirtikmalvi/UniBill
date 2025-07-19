using System;
using System.ComponentModel.DataAnnotations;

namespace UniBill.DTOs.CustomerDTOs;

public class CreateCustomerDTO
{
    [Required(ErrorMessage = "Customer Name is required.")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mobile Number is required.")]
    [RegularExpression(@"^[^0-2]{1}[1-9]{9}$", ErrorMessage = "Phone Number cannot start with 0, 1, 2 and must contain 10 digits only.")]
    public string MobileNumber { get; set; }

    [EmailAddress(ErrorMessage = "Email must be in valid format.")]
    public string? Email { get; set; }
    public int? BusinessId { get; set; }
}
