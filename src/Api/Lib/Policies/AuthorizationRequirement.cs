using NetCoreExample.Server.Models.DTOs.Shared;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreExample.Server.Api.Lib.Policies {
    public class AuthorizationRequirement : IAuthorizationRequirement {
        public readonly AuthorizationPolicyType Type;

        public AuthorizationRequirement(AuthorizationPolicyType type) => Type = type;
    }
}
