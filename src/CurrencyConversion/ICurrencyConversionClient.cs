using NetCoreExample.Server.CurrencyConversion.Models;
using System;
using System.Threading.Tasks;

namespace NetCoreExample.Server.CurrencyConversion
{
    public interface ICurrencyConversionClient : IDisposable
    {
        public Task<RateResponse> GetConversionRate(string currency);
    }
}
