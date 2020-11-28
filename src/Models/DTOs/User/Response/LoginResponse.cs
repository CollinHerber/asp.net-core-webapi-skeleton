using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreExample.Server.Models.DTOs.User.Response {
    public class LoginResponse {
        public string Token { get; set; }
        public long Id { get; set; }
        public long ExpiresAt { get; set; }
        public bool ResetPassword { get; set; }
        public bool TokenRequired { get; set; }
    }
}
