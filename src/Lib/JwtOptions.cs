using Microsoft.IdentityModel.Tokens;

namespace NetCoreExample.Server.Lib
{
    public class JwtOptions
    {
        public SigningCredentials SigningCredentials { get; set; }
    }
}
