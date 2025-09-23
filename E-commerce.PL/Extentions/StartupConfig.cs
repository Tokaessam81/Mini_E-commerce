namespace E_commerce.PL.Extentions
{
    public static class StartupConfig
    {
        public static void AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerDocumentation();
            services.AddDatabase(configuration.GetConnectionString("DefaultConnection")!, configuration);
            services.AddJwtAuth(configuration);
            services.AddApplicationLayerServices();
            services.AddCustomValidationErrors();
        }

        public static void UseApplicationPipeline(this WebApplication app)
        {
            app.UseSwaggerDocumentation();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowCors");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
