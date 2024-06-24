using ExchangeGood.API.Middleware;
using ExchangeGood.API.SignalR;
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
        services.AddSingleton<PresenceTracker>();
        return services;
    }
}