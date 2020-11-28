using System;

namespace NetCoreExample.Server.Models.DTOs.User.Request
{
    public class VerifyPhoneRequest
    {
        public String Token { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }

    }
}
