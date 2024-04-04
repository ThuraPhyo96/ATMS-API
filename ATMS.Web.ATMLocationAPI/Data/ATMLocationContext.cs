using ATMS.Web.Dto.Models;
using Microsoft.EntityFrameworkCore;


namespace ATMS.Web.ATMLocationAPI.Data
{
    public class ATMLocationContext : DbContext
    {
        public ATMLocationContext()
        {

        }

        public ATMLocationContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)  
                .AddJsonFile("appsettings.json")
                .Build();

            builder.UseSqlServer(configurationRoot.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Division> Divisions { get; set; }  
        public DbSet<Township> Townships { get; set; }

        public DbSet<BankName> BankNames { get; set; }
        public DbSet<BankBranchName> BankBranchNames { get; set; }
        public DbSet<ATMLocation> ATMLocations { get; set; }
    }
}
