using ATMS.Web.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace ATMS.Web.BankMvc.Data
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext()
        {

        }

        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Township> Townships { get; set; }

        public DbSet<BankName> BankNames { get; set; }
        public DbSet<BankBranchName> BankBranchNames { get; set; }
        public DbSet<ATMLocation> ATMLocations { get; set; }
    }
}
