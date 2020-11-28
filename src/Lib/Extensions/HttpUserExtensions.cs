using NetCoreExample.Server.Models;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace NetCoreExample.Server.Lib.Extensions
{
    public static class HttpUserExtensions
    {
        public static long GetId(this ClaimsPrincipal user) =>
            Convert.ToInt64(user.FindFirstValue(ClaimTypes.NameIdentifier));

        public static string GetIp(this ClaimsPrincipal user, IHeaderDictionary requestHeaders, HttpContext context)
        {
            return requestHeaders["Cf-Connecting-IP"].FirstOrDefault()?.Trim().Trim(',').Split(',').Select(s => s.Trim()).FirstOrDefault()
                ?? context?.Connection?.RemoteIpAddress.ToString();
        }
    }
}
