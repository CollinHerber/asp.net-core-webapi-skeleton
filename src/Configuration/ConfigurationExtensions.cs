using Microsoft.Extensions.Configuration;

namespace NetCoreExample.Server.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string DbConnectionString(this IConfiguration configuration)
            => configuration.GetConnectionString("NetCoreExampleDb") ?? configuration["NetCoreExampleDb"];

        public static string RedisConnectionString(this IConfiguration configuration) => configuration.GetConfig("RedisDb");

        public static string TelemetryKey(this IConfiguration configuration) => configuration.GetConfig("TelemetryKey");

        public static string JwtKey(this IConfiguration configuration) =>
            configuration.GetConfig("JwtKey");

        public static string NetCoreExampleBaseUrl(this IConfiguration configuration) =>
            configuration.GetConfig("NetCoreExampleBaseUrl");

        public static string DivicaSalesBaseUrl(this IConfiguration configuration) =>
            configuration.GetConfig("DivicaSalesBaseUrl");

        public static string AccKingsBaseUrl(this IConfiguration configuration) =>
            configuration.GetConfig("AccKingsBaseUrl");

        public static string ChicksXBaseUrl(this IConfiguration configuration) =>
            configuration.GetConfig("ChicksXBaseUrl");

        public static string ChicksAdminPanelBaseUrl(this IConfiguration configuration) =>
            configuration.GetConfig("ChicksAdminPanelBaseUrl");
        
        public static string GetIPQualityScoreApiKey(this IConfiguration configuration) =>
            configuration.IPQualityScore("ApiKey");

        private static string IPQualityScore(this IConfiguration configuration, string key) =>
            configuration.GetSectionConfig("IPQualityScore", key);

        public static string GetCoinbaseCommerceApiKey(this IConfiguration configuration) =>
            configuration.CoinbaseCommerce("ApiKey");

        public static string GetCoinbaseCommerceWebhookSigningKey(this IConfiguration configuration) =>
            configuration.CoinbaseCommerce("WebhookSigningKey");

        private static string CoinbaseCommerce(this IConfiguration configuration, string key) =>
            configuration.GetSectionConfig("CoinbaseCommerce", key);

        public static string GetSendGridValidationApiKey(this IConfiguration configuration) =>
            configuration.SendGrid("ValidationApiKey");

        public static string GetSendGridEmailApiKey(this IConfiguration configuration) =>
            configuration.SendGrid("EmailApiKey");

        private static string SendGrid(this IConfiguration configuration, string key) =>
            configuration.GetSectionConfig("SendGrid", key);

        public static string GetAuthyApiKey(this IConfiguration configuration) =>
            configuration.Twilio("AuthyApikey");

        public static string GetAccountSid(this IConfiguration configuration) =>
            configuration.Twilio("AccountSid");

        public static string GetAuthToken(this IConfiguration configuration) =>
            configuration.Twilio("AuthToken");

        public static string GetFromPhoneNumber(this IConfiguration configuration) =>
            configuration.Twilio("FromPhoneNumber");

        private static string Twilio(this IConfiguration configuration, string key) =>
            configuration.GetSectionConfig("Twilio", key);

        private static string Intercom(this IConfiguration configuration, string key) =>
        configuration.GetSectionConfig("Intercom", key);

        public static string GetIntercomIdentityVerificationSecret(this IConfiguration configuration) =>
            configuration.Intercom("IdentityVerficationSecret");

        public static string GetG2ASecretKey(this IConfiguration configuration) =>
            configuration.G2A("SecretKey");

        public static string GetG2AApiHash(this IConfiguration configuration) =>
            configuration.G2A("ApiHash");

        public static string GetG2AEmail(this IConfiguration configuration) =>
            configuration.G2A("Email");

        private static string G2A(this IConfiguration configuration, string key) =>
            configuration.GetSectionConfig("G2A", key);

        private static string Payop(this IConfiguration configuration, string key) =>
            configuration.GetSectionConfig("Payop", key);

        public static string GetPayopPublicKey(this IConfiguration configuration) =>
            configuration.Payop("PublicKey");

        public static string GetPayopPrivateKey(this IConfiguration configuration) =>
            configuration.Payop("PrivateKey");

        public static string GetPayopJwtKey(this IConfiguration configuration) =>
            configuration.Payop("JwtKey");

        private static string AWS(this IConfiguration configuration, string key) =>
            configuration.GetSectionConfig("AWS", key);

        public static string GetS3Key(this IConfiguration configuration) =>
            configuration.AWS("S3AccessKey");

        public static string GetS3KeyId(this IConfiguration configuration) =>
            configuration.AWS("S3SecretKeyId");

        private static string Google(this IConfiguration configuration, string key) =>
            configuration.GetSectionConfig("Google", key);

        public static string GetGoogleGeocodingKey(this IConfiguration configuration) =>
            configuration.Google("GeocodingApiKey");

        private static string GetSectionConfig(this IConfiguration configuration, string section, string key) =>
            configuration.GetSection(section).GetConfig(key);

        private static string Paysafe(this IConfiguration configuration, string key) =>
            configuration.GetSection("Paysafe").GetConfig(key);

        public static string GetPaysafeKey(this IConfiguration configuration) =>
            configuration.Paysafe("ApiKey");

        public static string GetPaysafeSecret(this IConfiguration configuration) =>
            configuration.Paysafe("ApiSecret");

        public static string GetPaysafeEnvironment(this IConfiguration configuration) =>
            configuration.Paysafe("Environment");

        public static string GetNetCoreExamplePaysafeCADAccount(this IConfiguration configuration) =>
            configuration.Paysafe("NetCoreExampleCAD");

        public static string GetNetCoreExamplePaysafeUSDAccount(this IConfiguration configuration) =>
            configuration.Paysafe("NetCoreExampleUSD");

        public static string GetDivicaSalesPaysafeCADAccount(this IConfiguration configuration) =>
            configuration.Paysafe("DivicaSalesCAD");

        public static string GetDivicaSalesPaysafeUSDAccount(this IConfiguration configuration) =>
            configuration.Paysafe("DivicaSalesUSD");

        public static string GetAccKingsPaysafeCADAccount(this IConfiguration configuration) =>
            configuration.Paysafe("AccKingsCAD");

        public static string GetAccKingsPaysafeUSDAccount(this IConfiguration configuration) =>
            configuration.Paysafe("AccKingsUSD");

        private static string Monzo(this IConfiguration configuration, string key) =>
            configuration.GetSectionConfig("Monzo", key);

        public static string GetMonzoJwtToken(this IConfiguration configuration) =>
            configuration.Monzo("JwtToken");

        private static string BlueSnap(this IConfiguration configuration, string key) =>
            configuration.GetSectionConfig("BlueSnap", key);
        public static string GetBlueSnapBaseUrl(this IConfiguration configuration) =>
            configuration.BlueSnap("BaseUrl");
        public static string GetBlueSnapApiUsername(this IConfiguration configuration) =>
            configuration.BlueSnap("ApiUsername");
        public static string GetBlueSnapApiPassword(this IConfiguration configuration) =>
            configuration.BlueSnap("ApiPassword");

        private static string Discord(this IConfiguration configuration, string key) =>
            configuration.GetSectionConfig("Discord", key);

        public static string GetDiscordAccountsSoldWebhookUrl(this IConfiguration configuration) =>
            configuration.Discord("AccountsSoldWebhookUrl");

        private static string GetConfig(this IConfiguration configuration, string key)
        {
            return configuration[key];
        }

        public static string GetPaysafeCADAccountNumber(this IConfiguration configuration, string websiteShortCode)
        {
            switch (websiteShortCode)
            {
                case "CG":
                    return GetNetCoreExamplePaysafeCADAccount(configuration);
                case "DS":
                    return GetDivicaSalesPaysafeCADAccount(configuration);
                default:
                    return GetNetCoreExamplePaysafeCADAccount(configuration);
            }
        }

        public static string GetPaysafeUSDAccountNumber(this IConfiguration configuration, string websiteShortCode)
        {
            switch (websiteShortCode)
            {
                case "CG":
                    return GetNetCoreExamplePaysafeUSDAccount(configuration);
                case "DS":
                    return GetDivicaSalesPaysafeUSDAccount(configuration);
                case "AK":
                    return GetAccKingsPaysafeUSDAccount(configuration);
                default:
                    return GetNetCoreExamplePaysafeUSDAccount(configuration);
            }
        }
    }
}
