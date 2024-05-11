using E_Commerce.HandelResponses;
using Infrastructure.BasketReposatory;
using Infrastructure.Interfaces;
using Infrastructure.Reposatories;
using Microsoft.AspNetCore.Mvc;
using Services.BasketServices.Interfaces;
using Services.BasketServices.Services;
using Services.BasketServices.Services.Dto;
using Services.CasheService.Interface;
using Services.CasheService.Service;
using Services.Interfaces;
using Services.OrderService.Interface;
using Services.OrderService.Services;
using Services.OrderService.Services.Dto;
using Services.Services;
using Services.Services.Dto;
using Services.StripePaymentService.Interface;
using Services.StripePaymentService.Services;
using Services.TokenService.Interface;
using Services.TokenService.Services;
using Services.UserService;

namespace E_Commerce.Excetensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IProductReposatory, ProductReposatory>();
            services.AddScoped<IBasketReposatory, BasketReposatory>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICasheService, CasheService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();




            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericReposatory<>), typeof(GenericReposatory<>));

            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(BasketProfile));
            services.AddAutoMapper(typeof(OrderProfile));

            
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actioncontext =>
                {
                    var errors = actioncontext.ModelState
                                              .Where(model => model.Value.Errors.Count > 0)
                                              .SelectMany(error => error.Value.Errors)
                                              .Select(error => error.ErrorMessage)
                                              .ToList();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);

                };
            });

            return services;
        }
    }
}
