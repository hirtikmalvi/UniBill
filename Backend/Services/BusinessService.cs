using Microsoft.EntityFrameworkCore;
using UniBill.Data;
using UniBill.DTOs;
using UniBill.DTOs.BusinessAddressDTOs;
using UniBill.DTOs.BusinessDTOs;
using UniBill.DTOs.BusinessTypeDTOs;
using UniBill.DTOs.UserDTOs;
using UniBill.Models;
using UniBill.Services.IServices;

namespace UniBill.Services
{
    public class BusinessService(AppDbContext context) : IBusinessService
    {
        public async Task<CustomResult<RegisterBusinessResponseDTO>> RegisterBusiness(RegisterBusinessDTO request)
        {
            if (!(await context.Users.AnyAsync(u => u.UserId == request.UserId)))
            {
                return CustomResult<RegisterBusinessResponseDTO>.Fail("Business Registration failed.", new List<string>
                {
                    $"User with UserId: {request.UserId} does not exits."
                });
            }

            if (await context.Businesses.AnyAsync(b => b.UserId == request.UserId))
            {
                return CustomResult<RegisterBusinessResponseDTO>.Fail("Business Registration failed.", new List<string>
                {
                    $"Could not create Business because Business associated with UserId: {request.UserId} already exists."
                });
            }

            if (!(await context.BusinessTypes.AnyAsync(bt => bt.BusinessTypeId == request.BusinessTypeId)))
            {
                return CustomResult<RegisterBusinessResponseDTO>.Fail("Business Registration failed.", new List<string>
                {
                    $"Could not create Business because BusinessType with ID:{request.BusinessTypeId} does not exists."
                });
            }

            var businessAddress = new BusinessAddress();
            businessAddress.ShopNo = request.ShopNo;
            businessAddress.Area = request.Area;
            businessAddress.Landmark = request.Landmark;
            businessAddress.Road = request.Road;
            businessAddress.City = request.City;
            businessAddress.State = request.State;
            businessAddress.Country = request.Country;
            businessAddress.PinOrPostalCode = request.PinOrPostalCode;

            var addedAddress = await context.BusinessAddresses.AddAsync(businessAddress);
            await context.SaveChangesAsync();

            var business = new Business();
            business.BusinessTypeId = request.BusinessTypeId;
            business.BusinessName = request.BusinessName;
            business.Phone = request.PhoneNo;
            business.UserId = request.UserId;
            business.BusinessAddressId = addedAddress.Entity.AddressId;

            var addedBusiness = await context.Businesses.AddAsync(business);
            await context.SaveChangesAsync();

            return CustomResult<RegisterBusinessResponseDTO>.Ok(new RegisterBusinessResponseDTO
            {
                UserId = addedBusiness.Entity.UserId,
                BusinessId = addedBusiness.Entity.BusinessId,
                BusinessName = addedBusiness.Entity.BusinessName,
                BusinessTypeId = addedBusiness.Entity.BusinessTypeId
            }, "Business Added Successfully.");
        }
        public async Task<CustomResult<GetBusinessDTO>> GetBusiness(int businessId)
        {
            if (!(await context.Businesses.AnyAsync(b => b.BusinessId == businessId)))
            {
                return CustomResult<GetBusinessDTO>.Fail($"Could not get Business.", new List<string>
                {
                    $"Business does not exists for BusinessId: {businessId}"
                });
            }
            var business = await context.Businesses.Where(b => b.BusinessId == businessId).Select(b => new GetBusinessDTO
            {
                BusinessId = b.BusinessId,
                BusinessName = b.BusinessName,
                Phone = b.Phone,
                User = new UserDTO
                {
                    UserId = b.User.UserId,
                    Email = b.User.Email
                },
                BusinessType = new BusinessTypeDTO
                {
                    BusinessTypeId = b.BusinessType.BusinessTypeId,
                    BusinessTypeName = b.BusinessType.Name
                },
                Address = new BusinessAddressDTO
                {
                    ShopNo = b.BusinessAddress.ShopNo,
                    Area = b.BusinessAddress.Area,
                    Landmark = b.BusinessAddress.Landmark,
                    Road = b.BusinessAddress.Road,
                    City = b.BusinessAddress.City,
                    State = b.BusinessAddress.State,
                    Country = b.BusinessAddress.Country,
                    PinOrPostalCode = b.BusinessAddress.PinOrPostalCode,

                }

            }).FirstOrDefaultAsync();

            return CustomResult<GetBusinessDTO>.Ok(business, "Business fetched successfully.");
        }
    }
}
