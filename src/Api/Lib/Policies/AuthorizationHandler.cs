using NetCoreExample.Server.Models.DTOs.Shared;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;

namespace NetCoreExample.Server.Api.Lib.Policies {
    public class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>, IAuthorizationHandler {
        private readonly UserResolver _userResolver;

        public AuthorizationHandler(UserResolver userResolver) => _userResolver = userResolver;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement) {
            var roles = await _userResolver.GetRoles();

            bool isAllowed = false;
            if (!roles.IsNullOrEmpty())
            {
                isAllowed = roles.Contains(Enum.GetName(typeof(AuthorizationPolicyType), requirement.Type));
            }

            if (isAllowed) {
                context.Succeed(requirement);
            } else {
                context.Fail();
            }
        }
    }
}
