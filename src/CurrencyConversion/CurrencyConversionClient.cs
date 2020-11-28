using NetCoreExample.Server.CurrencyConversion.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace NetCoreExample.Server.CurrencyConversion
{
    public class CurrencyConversionClient : ICurrencyConversionClient
    {
        private HttpClient _httpClient;

        private JsonSerializerSettings _jsonSerializerSettings;

        private readonly string _currencyConversionUrl;

        public CurrencyConversionClient()
        {
            _currencyConversionUrl = "https://api.exchangeratesapi.io/latest?base=USD&symbols=";
            SetupHttpClient();
            SetupJsonSerializer();
        }

        public async Task<RateResponse> GetConversionRate(string currency)
        {
            var url = _currencyConversionUrl + currency;

            var response = await _httpClient.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RateResponse>(body, _jsonSerializerSettings);
            PropertyInfo property = result.Rates.GetType().GetProperties().FirstOrDefault(x => x.Name == currency);
            var newRate = (decimal) property.GetValue(result.Rates);
            newRate = newRate * (decimal) 1.03;
            property.SetValue(result.Rates, newRate);
            return result;
        }

        private void SetupHttpClient()
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void SetupJsonSerializer()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
