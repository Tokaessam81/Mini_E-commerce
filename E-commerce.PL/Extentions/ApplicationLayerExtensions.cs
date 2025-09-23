using E_commerce.Application.Mappers;
using E_commerce.Application.Persistence;
using E_commerce.Application.Repository.Contract;
using E_commerce.Application.Service.Contract;
using E_commerce.Infrastructure.Persistence;
using E_commerce.Infrastructure.Repositories;
using E_commerce.Infrastructure.Services;

namespace E_commerce.PL.Extentions
{
    public static class ApplicationLayerExtensions
    {
        public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
        {
           services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService,AuthServices>();
            services.AddScoped<ITokenService, TokenServices>();
            services.AddScoped<IRefreshTokenRepo, RefreshTokenRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<IProductService, ProductServices>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowCors",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });


            return services;
        }
    }
}
