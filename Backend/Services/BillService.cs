using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniBill.Data;
using UniBill.DTOs;
using UniBill.DTOs.BillDTOs;
using UniBill.DTOs.CustomerDTOs;
using UniBill.DTOs.ItemDTOs;
using UniBill.Models;
using UniBill.Services.IServices;

namespace UniBill.Services
{
    //POST    /api/bills
    //GET     /api/bills
    //GET     /api/bills/{id}
    public class BillService(IHttpContextAccessor httpContextAccessor, AppDbContext context) : IBillService
    {
        public async Task<CustomResult<CreateBillResponseDTO>> CreateBill(CreateBillDTO request)
        {
            var user = httpContextAccessor.HttpContext?.User;
            var businessId = Convert.ToInt32(user.FindFirst("BusinessId")?.Value);


            if (request.BusinessId != null && businessId != request.BusinessId)
            {
                return CustomResult<CreateBillResponseDTO>.Fail("Could not create bill.", [
                    $"BusinessId mismatch."
                ]);
            }

            if (!(await context.Businesses.AnyAsync(b => b.BusinessId == businessId)))
            {
                return CustomResult<CreateBillResponseDTO>.Fail("Could not create bill.", [
                    $"Business does not exist for BusinessId: {businessId}."
                ]);
            }

            if (!(await context.Customers.AnyAsync(c => c.BusinessId == businessId && c.CustomerId == request.CustomerId)))
            {
                return CustomResult<CreateBillResponseDTO>.Fail("Could not create bill.", [
                    $"Customer does not exist for CustomerId: {request.CustomerId} for this Business with BusinessId: {businessId}. Kindly create Customer and then try to add."
                ]);
            }

            var itemIds = request.BillItems.Select(bi => bi.ItemId).ToList();

            var validItemIds = await context.Items.Where(i => i.BusinessId == businessId && itemIds.Contains(i.ItemId)).Select(i => i.ItemId).ToListAsync();

            if (itemIds.Except(validItemIds).Any())
            {
                return CustomResult<CreateBillResponseDTO>.Fail("Could not create bill.", [
                    $"Either Item does not exist or there is a problem with added Items. Try again after verifying."
                ]);
            }

            var bill = new Bill
            {
                Date = DateTime.Now,
                CustomerId = request.CustomerId,
                Discount = request.Discount,
                Tax = request.Tax,
                LabourCharges = (decimal)request.LabourCharges!,
                BusinessId = businessId,
                BillItems = request.BillItems.Select(bi => new BillItem
                {
                    ItemId = bi.ItemId,
                    Quantity = bi.Quantity,
                    Rate = bi.Rate
                }).ToList()
            };

            var addedBill = context.Bills.Add(bill).Entity;
            await context.SaveChangesAsync();

            return CustomResult<CreateBillResponseDTO>.Ok(new CreateBillResponseDTO
            {
                BillId = addedBill.BillId,
                Date = addedBill.Date,
                CustomerId = request.CustomerId,
                TotalBillItems = addedBill.BillItems.Count
            }, "Bill has been created.");
        }

        public async Task<CustomResult<List<GetAllBillResponseDTO>>> GetAllBills()
        {
            var user = httpContextAccessor.HttpContext?.User;
            var businessId = Convert.ToInt32(user.FindFirst("BusinessId")?.Value);

            var bills = await context.Bills.Where(b => b.BusinessId == businessId).Include(b => b.BillItems).Select(b => new GetAllBillResponseDTO
            {
                BillId = b.BillId,
                CustomerId = b.CustomerId,
                Customer = b.Customer.CustomerName,
                CustomerMobileNumber = b.Customer.MobileNumber,
                Date = b.Date,
                SubTotal = b.BillItems.Sum(bi => bi.Rate * bi.Quantity),
                Discount = (decimal)b.Discount!,
                Tax = (decimal)b.Tax!,
                LabourCharges = b.LabourCharges
            }).ToListAsync();

            if (bills.Count == 0)
            {
                return CustomResult<List<GetAllBillResponseDTO>>.Ok(bills, $"No Bills found for this Business with BusinessId: {businessId}");
            }
            return CustomResult<List<GetAllBillResponseDTO>>.Ok(bills, "Bills fetched successfully.");
        }

        public async Task<CustomResult<GetBillDTO>> GetBillById(int billId)
        {
            var user = httpContextAccessor.HttpContext?.User;
            var businessId = Convert.ToInt32(user.FindFirst("BusinessId")?.Value);

            // var bill = await context.Bills.FirstOrDefaultAsync(b => b.BusinessId == businessId && b.BillId == billId);
            var bill = await context.Bills.Where(b => b.BusinessId == businessId && b.BillId == billId)
                             .Include(b => b.Customer)
                             .Include(b => b.BillItems)
                                .ThenInclude(bi => bi.Item)
                                    .ThenInclude(i => i.Category)
                             .Include(b => b.BillItems)
                                .ThenInclude(bi => bi.Item)
                                    .ThenInclude(i => i.ItemType)
                             .Include(b => b.BillItems)
                                .ThenInclude(bi => bi.Item)
                                    .ThenInclude(i => i.Unit)
                             .FirstOrDefaultAsync();

            if (bill == null)
            {
                return CustomResult<GetBillDTO>.Fail("Could not get the  bill.", [
                    $"Bill does not exist for BillId: {billId}."
                ]);
            }

            var billToReturn = new GetBillDTO
            {
                BillId = bill.BillId,
                Date = bill.Date,
                CustomerId = bill.CustomerId,
                Discount = bill.Discount,
                Tax = bill.Tax,
                LabourCharges = bill.LabourCharges,
                Customer = new GetCustomerDTO
                {
                    CustomerId = bill.CustomerId,
                    CustomerName = bill.Customer.CustomerName,
                    MobileNumber = bill.Customer.MobileNumber,
                    Email = bill.Customer.Email,
                },
                BillItems = bill.BillItems.Select(bi => new GetBillItemDTO
                {
                    BillItemId = bi.BillItemId,
                    Quantity = bi.Quantity,
                    Rate = bi.Rate,
                    Total = bi.Total,
                    Item = new GetItemDTO
                    {
                        ItemId = bi.Item.ItemId,
                        ItemName = bi.Item.ItemName,
                        ItemRate = bi.Item.ItemRate,
                        CategoryId = bi.Item.CategoryId,
                        Category = bi.Item.Category.CategoryName,
                        UnitId = bi.Item.UnitId,
                        Unit = bi.Item.Unit.UnitName,
                        UnitShortName = bi.Item.Unit.ShortUnitName,
                        ItemTypeId = bi.Item.ItemTypeId,
                        ItemType = bi.Item.ItemType.Name
                    }
                }).ToList()
            };
            return CustomResult<GetBillDTO>.Ok(billToReturn, "Bill fetched successfully.");
        }
    }
}
