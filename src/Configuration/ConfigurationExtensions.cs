using Microsoft.Extensions.Configuration;

namespace NetCoreExample.Server.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string DbConnectionString(this IConfiguration configuration)
            => configuration.GetConnectionString("NetCoreExampleDb") ?? configuration["NetCoreExampleDb"];

        public static string TelemetryKey(this IConfiguration configuration) => configuration.GetConfig("TelemetryKey");

        public static string JwtKey(this IConfiguration configuration) =>
            configuration.GetConfig("JwtKey");

        private static string GetConfig(this IConfiguration configuration, string key)
        {
            return configuration[key];
        }
    }
}
