using ATMS.Web.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace ATMS.Web.API.Data
{
    public class ATMContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public ATMContext()
        {
        }

        public ATMContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Customer>().HasIndex(u => u.CustomerGuid).IsUnique();
            builder.Entity<Customer>().Property(x => x.CustomerGuid).HasDefaultValueSql("NEWID()");

            builder.Entity<BankAccount>().HasIndex(u => u.BankAccountGuid).IsUnique();
            builder.Entity<BankAccount>().Property(x => x.BankAccountGuid).HasDefaultValueSql("NEWID()");

            builder.Entity<BankCard>().HasIndex(u => u.BankCardGuid).IsUnique();
            builder.Entity<BankCard>().Property(x => x.BankCardGuid).HasDefaultValueSql("NEWID()");

            builder.Entity<BalanceHistory>().HasIndex(u => u.BalanceHistoryGuid).IsUnique();
            builder.Entity<BalanceHistory>().Property(x => x.BalanceHistoryGuid).HasDefaultValueSql("NEWID()");
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankCard> BankCards { get; set; }
        public DbSet<BalanceHistory> BalanceHistories { get; set; }
    }
}
