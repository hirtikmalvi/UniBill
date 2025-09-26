using Microsoft.EntityFrameworkCore;
using UniBill.Models;

namespace UniBill.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Business>().HasKey(b => b.BusinessId);
            modelBuilder.Entity<BusinessAddress>().HasKey(ba => ba.AddressId);
            modelBuilder.Entity<BusinessType>().HasKey(bt => bt.BusinessTypeId);
            modelBuilder.Entity<Unit>().HasKey(u => u.UnitId);
            modelBuilder.Entity<AllowedUnit>().HasKey(au => new
            {
                au.BusinessTypeId,
                au.UnitId
            });
            modelBuilder.Entity<ItemType>().HasKey(it => it.ItemTypeId);
            modelBuilder.Entity<AllowedItemType>().HasKey(ait => new
            {
                ait.BusinessTypeId,
                ait.ItemTypeId
            });
            modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<AllowedCategories>().HasKey(ac => new
            {
                ac.CategoryId,
                ac.BusinessTypeId
            });
            modelBuilder.Entity<Item>().HasKey(i => i.ItemId);
            modelBuilder.Entity<Customer>().HasKey(c => c.CustomerId);
            modelBuilder.Entity<Bill>().HasKey(b => b.BillId);
            modelBuilder.Entity<BillItem>().HasKey(bi => bi.BillItemId);




            modelBuilder.Entity<User>().HasOne(u => u.Business).WithOne(b => b.User).HasForeignKey<Business>(b => b.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Business>().HasOne(u => u.BusinessAddress).WithOne(ba => ba.Business).HasForeignKey<Business>(b => b.BusinessAddressId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BusinessType>().HasMany(bt => bt.AllowedUnits).WithOne(au => au.BusinessType).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AllowedUnit>().HasOne(au => au.BusinessType).WithMany(au => au.AllowedUnits).HasForeignKey(au => au.BusinessTypeId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AllowedItemType>().HasOne(ait => ait.BusinessType).WithMany(bt => bt.AllowedItemTypes).HasForeignKey(ait => ait.BusinessTypeId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AllowedItemType>().HasOne(ait => ait.ItemType).WithMany().HasForeignKey(ait => ait.ItemTypeId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>().HasOne(c => c.ItemType);

            modelBuilder.Entity<Item>().HasOne(i => i.Category).WithMany().HasForeignKey(i => i.CategoryId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(i => i.Unit).WithMany().HasForeignKey(i => i.UnitId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(i => i.ItemType).WithMany().HasForeignKey(i => i.ItemTypeId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(i => i.Business).WithMany().HasForeignKey(i => i.BusinessId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>().HasOne(c => c.Business).WithMany().HasForeignKey(c => c.BusinessId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>().HasOne(b => b.Customer).WithMany(c => c.Bills).HasForeignKey(b => b.CustomerId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>().HasMany(b => b.BillItems).WithOne(bi => bi.Bill).HasForeignKey(bi => bi.BillId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BillItem>().HasOne(i => i.Item).WithMany().HasForeignKey(bi => bi.ItemId).OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired(true);

            modelBuilder.Entity<BusinessAddress>().Property(ba => ba.Landmark).IsRequired(false);
            modelBuilder.Entity<BusinessAddress>().Property(ba => ba.Road).IsRequired(false);

            modelBuilder.Entity<BillItem>().Property(bi => bi.Total).HasComputedColumnSql("[Rate] * [Quantity]");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessAddress> BusinessAddresses { get; set; }
        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<AllowedUnit> AllowedUnits { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<AllowedItemType> AllowedItemTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AllowedCategories> AllowedCategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillItem> BillItems { get; set; }
    }
}
