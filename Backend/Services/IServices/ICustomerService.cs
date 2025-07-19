using System;
using UniBill.DTOs;
using UniBill.DTOs.CustomerDTOs;

namespace UniBill.Services.IServices;

public interface ICustomerService
{
    Task<CustomResult<GetCustomerDTO>> CreateCustomer(CreateCustomerDTO request);

    Task<CustomResult<List<GetCustomerDTO>>> GetAllCustomers();

    Task<CustomResult<GetCustomerDTO>> GetCustomerById(int id);

    Task<CustomResult<GetCustomerDTO>> GetCustomerByMobileNumber(string mobileNumber);
}
