using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using NetCoreExample.Server.Configuration;
using NetCoreExample.Server.Redis.Interfaces;
using NetCoreExample.Server.Redis.Repositories;

namespace NetCoreExample.Server.Redis {
    public static class Injection {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration) {
            services.AddSingleton<IRedisClientsManager>(c => new RedisManagerPool(configuration.RedisConnectionString()));
            services.AddScoped<IJwtRepository, JwtRepository>();
        }
    }
}
