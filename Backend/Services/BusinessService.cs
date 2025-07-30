using System.Security.Claims;
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
    public class BusinessService(AppDbContext context, CurrentUserContext currentUserContext, ITokenService tokenService) : IBusinessService
    {
        public async Task<CustomResult<RegisterBusinessResponseDTO>> RegisterBusiness(RegisterBusinessDTO request)
        {
            var userId = currentUserContext.UserId;
            var user = await context.Users.Include(u => u.Business).FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return CustomResult<RegisterBusinessResponseDTO>.Fail("Business Registration failed.", new List<string>
                {
                    $"User with UserId: {userId} does not exits."
                });
            }

            if (await context.Businesses.AnyAsync(b => b.UserId == userId))
            {
                return CustomResult<RegisterBusinessResponseDTO>.Fail("Business Registration failed.", new List<string>
                {
                    $"Could not create Business because Business associated with UserId: {userId} already exists."
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
            business.UserId = userId;
            business.BusinessAddressId = addedAddress.Entity.AddressId;

            var addedBusiness = await context.Businesses.AddAsync(business);
            await context.SaveChangesAsync();

            return CustomResult<RegisterBusinessResponseDTO>.Ok(new RegisterBusinessResponseDTO
            {
                UserId = addedBusiness.Entity.UserId,
                BusinessId = addedBusiness.Entity.BusinessId,
                BusinessName = addedBusiness.Entity.BusinessName,
                BusinessTypeId = addedBusiness.Entity.BusinessTypeId,
                AccessToken = tokenService.GenerateJwtToken(user)
            }, "Business Added Successfully.");
        }
        public async Task<CustomResult<GetBusinessDTO>> GetBusiness()
        {
            var businessId = currentUserContext.BusinessId;

            if (!currentUserContext.HasBusiness ||  !(await context.Businesses.AnyAsync(b => b.BusinessId == businessId)))
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

        public async Task<CustomResult<bool>> DoesUserHasBusiness()
        {
            var businessId = currentUserContext.BusinessId;
            var userId = currentUserContext.UserId;

            if (!currentUserContext.HasBusiness || !(await context.Businesses.AnyAsync(b => b.UserId == userId && b.BusinessId == businessId))) {
                return CustomResult<bool>.Fail("Business not found.", [
                    $"Could not found business associated with UserId: {userId}. Kindly register a business."
                ]);
            }
            return CustomResult<bool>.Ok(true, "Business found successfully.");
        }
    }
}
