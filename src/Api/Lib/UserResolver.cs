using NetCoreExample.Server.Data.Interfaces;
using NetCoreExample.Server.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace NetCoreExample.Server.Api.Lib {
    public class UserResolver {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepo;

        public UserResolver(IHttpContextAccessor httpContentAccessor, IUserRepository userRepo) {
            _httpContextAccessor = httpContentAccessor;
            _userRepo = userRepo;
        }

        public long GetUserId() {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrWhiteSpace(userId))
                return Convert.ToInt32(userId);
            else
                return 0;
        }

        public string GetUserIp() {
            return _httpContextAccessor.HttpContext?.Request?.Headers["X-Forwarded-For"].FirstOrDefault()?.Trim().Trim(',').Split(',').Select(s => s.Trim()).FirstOrDefault()
                ?? _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress.ToString();
        }

        public string GetUserAgent() {
            return _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"];
        }

        public async Task<User> GetUser() => await _userRepo.GetByIdAsync(GetUserId());

        public async Task<List<string>> GetRoles()
        {
            var userId = GetUserId();
            if (userId != 0)
            {
                return (await _userRepo.GetRolesAsync(userId)).ToList();
            }

            return null;
        }

        public async Task<bool> HasRole(string role) { 
            var roles = await GetRoles();
            return roles.Contains(role);
        }
    }
}
