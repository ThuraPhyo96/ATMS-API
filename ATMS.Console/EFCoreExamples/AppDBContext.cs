using ATMS.Web.Dto.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS.ConsoleApp.EFCoreExamples
{
    public class AppDBContext : DbContext
    {
        // DB first migration => firstly create DB with tables
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new()
            {
                DataSource = "DESKTOP-KGKMSQ5",
                InitialCatalog = "ATM_API_DB",
                UserID = "sa",
                Password = "Mingalar1"
            };

            optionsBuilder.UseSqlServer(sqlConnectionStringBuilder.ConnectionString);
        }

        public DbSet<BankAccount> BankAccounts { get; set; } = null!;
        public DbSet<BankCard> BankCards { get; set; } = null!;
        public DbSet<BalanceHistory> BalanceHistories { get; set; } = null!;

    }
}
