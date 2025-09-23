using E_commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace E_commerce.PL.Extentions
{
    public static class DatabaseServiceExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString, IConfiguration config)
        {
            services.AddDbContext<ECommerceDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }
    }
}
