using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExchangeGood.DAO.Extensions {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddDAOLayer(this IServiceCollection services, IConfiguration configuration) {
            // Add DBContext
            services.AddDbContext<GoodsExchangeContext>(opt => {
                // configure the options for this Database
                opt.UseSqlServer(configuration["ConnectionString:DefaultConnection"]);
            });
            // Add UnitOfWork
            services.AddScoped<ProductDAO, ProductDAO>();
            services.AddScoped<MemberDAO, MemberDAO>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
