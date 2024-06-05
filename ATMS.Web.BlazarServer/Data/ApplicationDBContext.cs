using ATMS.Web.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace ATMS.Web.BlazarServer.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SpendTransaction> SpendTransactions { get; set; }
    }
}
