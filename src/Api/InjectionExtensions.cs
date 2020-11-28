using NetCoreExample.Server.Api.Lib;
using NetCoreExample.Server.CurrencyConversion;
using NetCoreExample.Server.Data.Interfaces;
using NetCoreExample.Server.Data.Repositories;
using NetCoreExample.Server.Services;
using NetCoreExample.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace NetCoreExample.Server.Lib.Extensions
{
    public static class InjectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrencyConversionClient, CurrencyConversionClient>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<UserResolver>();
        }
    }
}
