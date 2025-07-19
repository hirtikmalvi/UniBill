using System;
using Microsoft.EntityFrameworkCore;
using UniBill.Data;

namespace UniBill.Helper;

public class HelperClass(AppDbContext context)
{
    public async Task<bool> IsCustomerMobileNumberExists(int businessId, string mobileNo)
    {
        return await context.Customers.AnyAsync(c => c.Business.BusinessId == businessId && c.MobileNumber == mobileNo);
    }
}
