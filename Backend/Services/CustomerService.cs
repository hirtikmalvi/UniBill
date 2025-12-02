using System;
using Microsoft.EntityFrameworkCore;
using UniBill.Data;
using UniBill.DTOs;
using UniBill.DTOs.CustomerDTOs;
using UniBill.Models;
using UniBill.Services.IServices;

namespace UniBill.Services;

public class CustomerService(AppDbContext context, CurrentUserContext currentUserContext) : ICustomerService
{
    public async Task<CustomResult<GetCustomerDTO>> CreateCustomer(CreateCustomerDTO request)
    {
        var businessId = currentUserContext.BusinessId;
        
        var customer = await context.Customers.Where(c => c.Business.BusinessId == businessId && c.MobileNumber == request.MobileNumber).FirstOrDefaultAsync();

        if (customer != null)
        {
            return CustomResult<GetCustomerDTO>.Ok(new GetCustomerDTO
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                MobileNumber = customer.MobileNumber,
                Email = customer.Email
            }, $"Customer already exists with Mobile No. {request.MobileNumber} in your business.");
        }

        var customerToAdd = new Customer
        {
            CustomerName = request.CustomerName,
            MobileNumber = request.MobileNumber,
            Email = request.Email,
            BusinessId = businessId
        };

        var addedCustomer = await context.Customers.AddAsync(customerToAdd);
        await context.SaveChangesAsync();

        return CustomResult<GetCustomerDTO>.Ok(new GetCustomerDTO
        {
            CustomerId = addedCustomer.Entity.CustomerId,
            CustomerName = addedCustomer.Entity.CustomerName,
            MobileNumber = addedCustomer.Entity.MobileNumber,
            Email = addedCustomer.Entity.Email
        }, "Customer added successfully.");
    }

    public async Task<CustomResult<List<GetCustomerDTO>>> GetAllCustomers()
    {
        var businessId = currentUserContext.BusinessId;

        if (!(await context.Customers.AnyAsync(c => c.BusinessId == businessId)))
        {
            return CustomResult<List<GetCustomerDTO>>.Fail("Could not get Customers.", new List<string>
            {
                $"There are no Customers for Business with BusinessId: {businessId}"
            });
        }

        var result = await context.Customers.Where(c => c.BusinessId == businessId).Select(c => new GetCustomerDTO
        {
            CustomerId = c.CustomerId,
            CustomerName = c.CustomerName,
            MobileNumber = c.MobileNumber,
            Email = c.Email
        }).ToListAsync();

        return CustomResult<List<GetCustomerDTO>>.Ok(result, "Customers fetched sucessfully.");
    }

    public async Task<CustomResult<GetCustomerDTO>> GetCustomerById(int id)
    {
        var businessId = currentUserContext.BusinessId;
        
        var customer = await context.Customers.FirstOrDefaultAsync(c => c.BusinessId == businessId && c.CustomerId == id);

        if (customer == null)
        {
            return CustomResult<GetCustomerDTO>.Fail("Cannot get Customer.", new List<string>
            {
                $"Customer with CustomerId: {id} does not exist in your business."
            });
        }
        return CustomResult<GetCustomerDTO>.Ok(new GetCustomerDTO
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.CustomerName,
            MobileNumber = customer.MobileNumber,
            Email = customer.Email
        }, "Customer fetched successfully.");
    }
    
    public async Task<CustomResult<GetCustomerDTO>> GetCustomerByMobileNumber(string mobileNumber)
    {
        var businessId = currentUserContext.BusinessId;

        var customer = await context.Customers.FirstOrDefaultAsync(c => c.BusinessId == businessId && c.MobileNumber == mobileNumber);

        if (customer == null)
        {
            return CustomResult<GetCustomerDTO>.Fail("Cannot get Customer.", new List<string>
            {
                $"Customer with MobileNumber: {mobileNumber} does not exist in your business."
            });
        }
        return CustomResult<GetCustomerDTO>.Ok(new GetCustomerDTO
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.CustomerName,
            MobileNumber = customer.MobileNumber,
            Email = customer.Email
        }, "Customer fetched successfully.");
    }
    
}
