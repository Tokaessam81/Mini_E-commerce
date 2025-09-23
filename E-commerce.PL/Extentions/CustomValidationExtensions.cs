using E_commerce.PL.Errors;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.PL.Extentions
{
    public static class CustomValidationExtensions
    {
        public static IServiceCollection AddCustomValidationErrors(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState
                                        .Where(p => p.Value!.Errors.Count > 0)
                                        .SelectMany(p => p.Value!.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .ToList();

                    var response = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
    }
}
