using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreExample.Server.Models.DTOs.User.Request
{
    public class UpdatePasswordRequest
    {
        public string CurrentPassword {get; set;}
        public string NewPassword {get; set;}
    }
}
