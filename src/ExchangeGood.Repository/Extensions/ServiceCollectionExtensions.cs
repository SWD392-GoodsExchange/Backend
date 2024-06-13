using ExchangeGood.Repository.Interfaces;
using ExchangeGood.Repository.Mapper;
using ExchangeGood.Repository.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Extensions {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddRepositoryLayer(this IServiceCollection services) {
            services.AddAutoMapper(typeof(ServiceProfile));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            return services; 
        }
    }
}
