using System.Runtime.CompilerServices;
using ExchangeGood.API.OptionsSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ExchangeGood.API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection IdentityServices(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        return services;
    }
    
}