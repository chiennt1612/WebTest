using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFramework.API.Helper
{
    public static class DatabaseExtensions
    {
        public static void RegisterDbContexts<TUserDbContext, TDbContext>
                   (this IServiceCollection services, IConfiguration configuration, string migrationsAssembly)
            where TDbContext : DbContext
            where TUserDbContext : DbContext
        {
            // Config DB for identity
            services.AddDbContext<TUserDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Config DB for App
            services.AddDbContext<TDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AppConnection"), sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
}
