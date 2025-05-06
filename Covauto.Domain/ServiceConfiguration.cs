using Covauto.Domain.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Covauto.Domain
{
    public static class ServiceConfiguration
    {
        public static void RegisterServices(IServiceCollection services, string connectionString)
        {
            services.AddSqlite<AppDbContext>(connectionString);
        }
    }
}