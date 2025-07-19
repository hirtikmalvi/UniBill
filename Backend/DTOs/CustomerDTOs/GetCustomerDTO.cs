using System;
using UniBill.Models;

namespace UniBill.DTOs.CustomerDTOs;

public class GetCustomerDTO
{
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string? Email {  get; set; }
}
