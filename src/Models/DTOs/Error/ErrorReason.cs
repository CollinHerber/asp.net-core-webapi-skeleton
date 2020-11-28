using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreExample.Server.Models.DTOs.Error {
    public enum ErrorReason {
        Unauthorized,
        ServerError,
        ValidationError,
        InvalidData
    }
}
