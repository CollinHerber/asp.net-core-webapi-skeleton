using NetCoreExample.Server.Lib.Extensions;
using NetCoreExample.Server.Models;
using NetCoreExample.Server.Models.DTOs.User.Request;
using NetCoreExample.Server.Models.DTOs.User.Response;
using NetCoreExample.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Lib.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetCoreExample.Server.Models.DTOs.Shared;

namespace NetCoreExample.Server.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAvailableRoles")]
        [ProducesResponseType(typeof(List<UserEmployeeDto>), 200)]
        public async Task<IActionResult> GetAvailableRoles()
        {
            return Ok(Enum.GetNames(typeof(AuthorizationPolicyType)));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(Policy = nameof(AuthorizationPolicyType.DeleteUser))]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var deleted = await _userService.DeleteUser(id);
            if (deleted == false)
            {
                return BadRequest("Failed to delete");
            }
            return Ok(deleted);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserEmployeeDto), 200)]
        [Authorize(Policy = nameof(AuthorizationPolicyType.ViewCustomerDetails))]
        public async Task<IActionResult> GetById(long id)
        {
            var user = await _userService.GetById(id);

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("CheckEmail")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CheckEmail(string email)
        {
            return Ok(await _userService.CheckEmail(email));
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        public async Task<IActionResult> Register([FromBody]RegisterUserDto user)
        {
            if (user.Email == null)
            {
                return BadRequest("Email Required");
            }
            user.Ip = User.GetIp(Request?.Headers, HttpContext);
            var loginResponse = await _userService.CreateUser(user);

            if (loginResponse == null)
            { 
                return BadRequest("Failed to Register");
            }

            return Ok(loginResponse);
        }

        [AllowAnonymous]
        [HttpPost("ByEmail")]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        public async Task<IActionResult> RegisterByEmail([FromBody]RegisterUserDto data)
        {
            data.Ip = User.GetIp(Request?.Headers, HttpContext);
            var createdUser = await _userService.CreateUserByEmail(data);

            if (createdUser == null)
            { 
                return BadRequest("Failed to Register");
            }

            return Ok(createdUser);
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserEmployeeDto), 200)]
        [Authorize(Policy = nameof(AuthorizationPolicyType.ViewCustomerDetails))]
        public async Task<IActionResult> UpdateUser(UserEmployeeDto user)
        {
            var updatedUser = await _userService.UpdateUser(user);

            if (updatedUser == null)
            {
                return BadRequest("No User Found To Update");
            }

            return Ok(updatedUser);
        }

        [HttpPut("UpdateInformation")]
        [ProducesResponseType(typeof(UserCustomerDto), 200)]
        public async Task<IActionResult> PatchUser([FromBody]UpdateInformationRequest data)
        {
            var user = await _userService.UpdateInformationById(User.GetId(), data);

            if (user == null)
            {
                return BadRequest("Failed To Update User");
            }

            return Ok(user);
        }

        [HttpPost("UpdatePassword")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> UpdatePassword([FromBody]UpdatePasswordRequest data)
        {
            var user = await _userService.UpdatePassword(User.GetId(), data);

            if (user == false)
            {
                return BadRequest("Failed to update password");
            }

            return Ok(user);
        }

        [HttpPost("{userId}/UpdatePassword")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(Policy = nameof(AuthorizationPolicyType.ResetPasswords))]
        public async Task<IActionResult> UpdatePasswordByAdmin([FromRoute] long userId, [FromBody]UpdatePasswordRequest passwordRequest) {
            var successful = await _userService.UpdatePasswordAdmin(userId, passwordRequest);
            if (successful)
            {
                return Ok(true);
            }
            return BadRequest("Failed to update password");
        }

        [HttpGet("RequestTwoFactorAuthentication")]
        [ProducesResponseType(typeof(AuthenticatorResponse), 200)]
        public async Task<IActionResult> RequestTwoFactorCode() {
            var code = await _userService.GenerateTotpData(User.GetId());

            return Ok(code);
        }

        [HttpPost("ConfirmTwoFactorActivation")]
        [ProducesResponseType(typeof(AuthenticatorStatusResponse), 200)]
        public async Task<IActionResult> VerifyAndEnable2Fa([FromBody] AuthenticatorRequest request) {
            var status = await _userService.VerifyAndAdd2Fa(User.GetId(), request);
            return Ok(status);
        }

        [HttpGet("CheckIfTwoFactorEnabled")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> Check2FaEnabled() {
            var isEnabled = await _userService.Check2FaEnabled(User.GetId());
            return Ok(isEnabled);
        }

        [HttpGet("CheckIfOpted")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CheckOptIn()
        {
            var isOptedIn = await _userService.CheckOptIn(User.GetId());
            if (isOptedIn.HasValue)
            {
               return Ok(isOptedIn);
            }
            return Ok(false);
        }

        [HttpPost("RemoveTwoFactorAuthentication")]
        [ProducesResponseType(typeof(AuthenticatorStatusResponse), 200)]
        public async Task<IActionResult> Remove2Fa([FromBody] AuthenticatorRequest request) {
            var status = await _userService.Remove2Fa(User.GetId(), request);
            return Ok(status);
        }

        [HttpPost("RemoveTwoFactorByAdmin/{userId}")]
        [ProducesResponseType(typeof(AuthenticatorStatusResponse), 200)]
        [Authorize(Policy = nameof(AuthorizationPolicyType.ResetPasswords))]
        public async Task<IActionResult> Remove2FaByAdmin([FromRoute] long userId) {
            var status = await _userService.Remove2FaByAdmin(userId);
            return Ok(status);
        }

        [HttpPost("UpdateEmail")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> UpdateEmail([FromBody]UpdateEmailRequest emailRequest) {
            var successful = await _userService.UpdateEmail(User.GetId(), emailRequest.Email);
            if (successful)
            {
                return Ok(true);
            }
            return Ok(false);
        }

        [HttpPost("{userId}/UpdateEmail")]
        [ProducesResponseType(typeof(bool), 200)]
        [Authorize(Policy = nameof(AuthorizationPolicyType.ResetEmails))]
        public async Task<IActionResult> UpdateEmail([FromRoute] long userId, [FromBody]UpdateEmailRequest emailRequest) {
            try
            {
                await _userService.UpdateEmail(userId, emailRequest.Email);
            } catch(EmailAlreadyExistsException e) {
                return BadRequest(e);
            }
            return Ok(true);

        }
    }
}
