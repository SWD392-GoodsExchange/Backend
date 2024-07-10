using ExchangeGood.Service.Interfaces;
using ExchangeGood.Service.UseCase;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeGood.Service.Authentication;
using ExchangeGood.Service.Services;
using Microsoft.Extensions.Configuration;

namespace ExchangeGood.Service.Extensions {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services, IConfiguration config) {
            services.AddScoped<IProductService, ProductService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IReportService, ReportService>();
			services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IAuthService, AuthService>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.Configure<VnPaySettings>(config.GetSection("VnPay"));
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IClaimsPrincipalExtensions, ClaimsPrincipalExtensions>();
            
            return services;
        }
    }
}
