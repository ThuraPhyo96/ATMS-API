using ATMS.Web.BankMvc.Data;
using Microsoft.EntityFrameworkCore;

namespace ATMS.Web.BankMvc
{
    public static class DBExtensionHelper
    {
        public static void RegisterDBContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(connectionString);
            },
            optionsLifetime: ServiceLifetime.Transient,
            contextLifetime: ServiceLifetime.Transient);
        }
    }
}
