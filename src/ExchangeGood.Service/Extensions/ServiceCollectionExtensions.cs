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

namespace ExchangeGood.Service.Extensions {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services) {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            return services;
        }
    }
}
