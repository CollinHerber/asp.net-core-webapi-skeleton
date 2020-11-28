using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreExample.Server.Models.DTOs.User.Response {
    public class AuthenticatorResponse {
        public string SharedKey { get; set; }
        public string AuthenticatorUri { get; set; }
    }
}
