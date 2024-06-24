using ExchangeGood.API.Middleware;
using FluentValidation;

namespace ExchangeGood.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
    IConfiguration config)
    {
        services.AddCors();
        services.AddSignalR();
        services.AddScoped<ExecuteValidation>();
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
        return services;
    }
}