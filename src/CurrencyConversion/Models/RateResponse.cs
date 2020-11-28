using System.Collections.Generic;

namespace NetCoreExample.Server.CurrencyConversion.Models
{
    public class RateResponse
    {
        public string Base {get; set;}
        public string Date {get; set;}
        public Rates Rates {get; set;}
    }
}
