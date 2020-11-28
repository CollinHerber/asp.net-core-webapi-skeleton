using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using NetCoreExample.Server.Lib.Extensions;
using NetCoreExample.Server.Models;
using NetCoreExample.Server.Models.DTOs.User.Request;
using NetCoreExample.Server.Models.DTOs.User.Response;
using NetCoreExample.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreExample.Server.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        public async Task<IActionResult> Login(RegisterUserDto loginRequest)
        {
            loginRequest.Ip = User.GetIp(Request?.Headers, HttpContext);
            Request.Headers.TryGetValue("Origin", out var originValues);
            var loginResponse = await _userService.Authenticate(loginRequest.Email, loginRequest.Password, loginRequest.Token, loginRequest.Ip, originValues.Any(x => x.Contains("admin.NetCoreExample.com") || x.Contains("qa-admin.NetCoreExample.com")));
            if (loginResponse == null)
            {
                throw new InvalidCredentialException("Invalid Email or Password");
            }

            return Ok(loginResponse);
        }

        [HttpDelete("Logout")]
        public IActionResult Logout() {
            var userId = User.GetId();
            _userService.Logout(userId);
            return Ok(true);
        }

        [Authorize]
        [HttpGet("Profile")]
        [ProducesResponseType(typeof(UserCustomerDto), 200)]
        public async Task<IActionResult> Profile()
        {
            var userId = User.GetId();
            var user = await _userService.GetProfile(userId);

            return Ok(user);
        }

        [Authorize]
        [HttpGet("RefreshToken")]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        public async Task<IActionResult> RefreshToken() {
            var loginResponse = await _userService.RefreshToken(User.GetId());
            return Ok(loginResponse);
        }

        [HttpPost("RequestResetPassword")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> RequestPasswordReset([FromBody]RequestResetPasswordRequest data)
        {
            var user = await _userService.FindByEmail(data);

            if (user == null)
            {
                return Ok(true);
            }

            await _userService.SendResetPasswordEmail(data);

            return Ok(true);
        }

        [HttpPost("ResetPassword")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordRequest data)
        {
            var status = await _userService.ResetPasswordByToken(data);

            return Ok(status);
        }
    }
}
