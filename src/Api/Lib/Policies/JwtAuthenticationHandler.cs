using NetCoreExample.Server.Lib.Extensions;
using Lib.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace NetCoreExample.Server.Api.Lib.Policies {
    public class JwtAuthenticationHandler : JwtBearerHandler {
        private readonly UserResolver _userResolver;

        public JwtAuthenticationHandler(UserResolver userResolver, IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock) {
            _userResolver = userResolver;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            var authHeader = Context.Request.Headers["Authorization"];
            var queryToken = Context.Request.Query["access_token"];
            var token = string.IsNullOrWhiteSpace(authHeader) ? queryToken.ToString() : authHeader.ToString()?.Split(" ")?.Last();
            var result = await base.HandleAuthenticateAsync();

            return result;
        }
    }
}
