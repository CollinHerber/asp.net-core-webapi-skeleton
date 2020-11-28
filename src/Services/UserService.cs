using System.Threading.Tasks;
using NetCoreExample.Server.Models;
using NetCoreExample.Server.Services.Interfaces;
using System.Collections.Generic;
using System.Text;
using System;
using Microsoft.AspNetCore.Identity;
using DevExtreme.AspNet.Data.ResponseModel;
using NetCoreExample.Server.Models.DTOs.User.Response;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Microsoft.Extensions.Configuration;
using NetCoreExample.Server.Configuration;
using NetCoreExample.Server.Data.Interfaces;
using NetCoreExample.Server.Models.DTOs.User.Request;
using System.Web;
using System.Linq;
using System.Security.Cryptography;
using Lib.Exceptions;

namespace NetCoreExample.Server.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<User> _signinManager;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepo;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository userRepo,
            SignInManager<User> signinManager,
            IConfiguration config,
            UserManager<User> userManager
        ){
            _userRepo = userRepo;
            _signinManager = signinManager;
            _config = config;
            _userManager = userManager;
        }

        public async Task<LoginResponse> Authenticate(string email, string password, string token, string ip, bool isAdmin = false)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = await _userRepo.GetByEmailAsync(email);

            if (user == null)
                return null;

            var roles = await _userRepo.GetRolesAsync(user.Id);

            //Do not allow users without roles into admin panel
            if(!roles.Any() && isAdmin) {
                return null;
            }

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                await _userManager.UpdateSecurityStampAsync(user);
                await SendResetPasswordEmail(new RequestResetPasswordRequest{Email = user.Email});
                return new LoginResponse {
                    Id = user.Id,
                    ResetPassword = true
                };
            }

            var result = await _signinManager.PasswordSignInAsync(email, password.Trim(), false, true);
            if (!result.Succeeded) {
                if (result.RequiresTwoFactor) {
                    if (string.IsNullOrEmpty(token)) {
                        return new LoginResponse {
                            TokenRequired = true
                        };
                    }

                    if(!await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, token))
                        return null;
                } else
                    return null;
            }

            //TODO: Do lockout check
            user.LastLoggedInIp = ip;
            await _userRepo.UpdateAsync(user);
            return new LoginResponse {
                Token = GenerateJwtToken(user, roles.FirstOrDefault()),
                ExpiresAt = roles.Any() ? DateTimeOffset.UtcNow.AddMinutes(90).ToUnixTimeMilliseconds() : DateTimeOffset.UtcNow.AddMinutes(40320).ToUnixTimeMilliseconds(),
                Id = user.Id
            };
        }

        public async Task<LoginResponse> RefreshToken(long id) {
            var user = await _userRepo.GetByIdAsync(id);
            var roles = await _userRepo.GetRolesAsync(id);

            return new LoginResponse {
                Token = GenerateJwtToken(user, roles.FirstOrDefault()),
                ExpiresAt = roles.Any() ? DateTimeOffset.UtcNow.AddMinutes(90).ToUnixTimeMilliseconds() : DateTimeOffset.UtcNow.AddMinutes(40320).ToUnixTimeMilliseconds(),
                Id = user.Id
            };
        }

        private string GenerateJwtToken(User user, string role) {
            var expires = !string.IsNullOrWhiteSpace(role) ? DateTime.UtcNow.AddMinutes(90) : DateTime.UtcNow.AddMinutes(40320);

            var payload = new JwtPayload {
                { "sub", user.Id.ToString() },
                { "jti", Guid.NewGuid().ToString() },
                { "id", user.Id.ToString() },
                { "exp", (int)expires.Subtract(new DateTime(1970, 1, 1)).TotalSeconds }
            };
            
            payload.Add(ClaimTypes.Role, role);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.JwtKey()));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(new JwtHeader(creds), payload);

            var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

#if !DEBUG
            _jwtRepository.Add(stringToken, user.Id);
#endif

            return stringToken;
        }

        public void Logout(long userId) {
#if !DEBUG
            _jwtRepository.RemoveByUserId(userId);
#endif
        }

        public async Task<LoginResponse> CreateUser(RegisterUserDto registerUser)
        {
            var existingUser = await _userRepo.GetByEmailAsync(registerUser.Email);

            if (existingUser != null)
            {
                return null;
            }

            var user = new User();
            user.UserName = user.Email;
            user.Password = null;
            user.Email = registerUser.Email;

            await _userRepo.CreateAsync(user, registerUser.Password);
            return await Authenticate(registerUser.Email, registerUser.Password, null, registerUser.Ip);
        }

        public async Task<bool> UpdatePassword(long id, UpdatePasswordRequest resetPasswordRequest)
        {
            var user = await _userRepo.GetByIdAsync(id);

            if (await _userManager.CheckPasswordAsync(user, resetPasswordRequest.CurrentPassword))
            {
                await _userRepo.UpdatePasswordAsync(id, resetPasswordRequest.NewPassword);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdatePasswordAdmin(long id, UpdatePasswordRequest resetPasswordRequest)
        {
            if (!string.IsNullOrWhiteSpace(resetPasswordRequest.NewPassword))
            {
                await _userRepo.UpdatePasswordAsync(id, resetPasswordRequest.NewPassword);
                return true;
            }
            return false;
        }

        public async Task<LoginResponse> CreateUserByEmail(RegisterUserDto userData)
        {
            var existingUser = await _userRepo.GetByEmailAsync(userData.Email);

            if (existingUser != null)
            {
                return null;
            }

            User user = new User();
            user.Email = userData.Email;
            user.FirstName = GetEmailPrefix(user.Email);
            user.OptedInForEmails = userData.OptedInForEmails;
            string unencryptedPassword = GeneratePassword(12);

            user.UserName = user.Email;
            user.LastLoggedInIp = userData.Ip;
            await _userRepo.CreateAsync(user, unencryptedPassword.Trim());

            if (user.Id != 0)
            {
                return await Authenticate(user.Email, unencryptedPassword, null, userData.Ip);
            }
            return null;
        }

        public async Task<User> FindByEmail(RequestResetPasswordRequest data)
        {
            return await _userRepo.GetByEmailAsync(data.Email);
        }

        public async Task<bool> SendResetPasswordEmail(RequestResetPasswordRequest data) {
            var user = await _userRepo.GetByEmailAsync(data.Email);

            if (user == null)
            {
                return true;
            }

            var token = await _userRepo.GeneratePasswordResetTokenAsync(user.Id);

            return true;
        }

        public async Task<bool> ResetPasswordByToken(ResetPasswordRequest request) {
            return await _userRepo.ResetPasswordAsync(request.Email, request.NewPassword, Base64UrlEncoder.Decode(request.AccessToken));
        }

        public async Task<User> UpdateUser(UserEmployeeDto user)
        {
            var existingUser = await _userRepo.GetByIdAsync(user.Id);

            if (existingUser == null)
            {
                return null;
            }


            await _userRepo.UpdateAsync(existingUser, user.Roles.ToArray());

            return existingUser;
        }

        public async Task<User> GetById(long id)
        {
            var toReturn = await _userRepo.GetByIdAsync(id);
            return toReturn;
        }

        public async Task<User> GetProfile(long id)
        {
            var user = await _userRepo.GetByIdAsync(id);

            if (user != null)
            {
                return user;
            }
            throw new EntityNotFoundException("User not found on profile route - ID: " + id);
        }

        public async Task<bool> CheckEmail(string email)
        {
            var user = await _userRepo.GetByEmailAsync(email);

            return user != null;
        }

        public async Task<bool> VerifyPhoneNumber(VerifyPhoneRequest data, long id)
        {
            var user = await _userRepo.GetByIdAsync(id);

            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }

            throw new InvalidDataException("Incorrect code entered, please check your code and try again.");
        }
        public async Task<User> UpdateInformationById(long id, UpdateInformationRequest data)
        {
            var user = await _userRepo.GetByIdAsNoTracking(id);

            if (user == null) {
                return null;
            }

            if (data.Email != null && user.Email != data.Email)
            {
                if(await CheckEmail(data.Email))
                {
                    return null;
                }
            }

            if(!string.IsNullOrWhiteSpace(data.FirstName))
                user.FirstName = data.FirstName;

            if(!string.IsNullOrWhiteSpace(data.LastName))
                user.LastName = data.LastName;

            if (!string.IsNullOrWhiteSpace(data.Email)) {
                user.UserName = data.Email;
                user.Email = data.Email;
            }

            if (!string.IsNullOrWhiteSpace(data.AvatarImagePath)) {
                user.AvatarImagePath = data.AvatarImagePath;
            }

            user.OptedInForEmails = data.OptedInForEmails;

            await _userRepo.UpdateAsync(user);

            return user;
        }

        private string GeneratePassword(int size)
        { 
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            char ch;  
            for (int i = 0; i < size; i++)  
            {  
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));  
                builder.Append(ch);  
            }  
            return builder + "123"; 
        }

        public async Task<AuthenticatorResponse> GenerateTotpData(long id) {
            var user = await _userRepo.GetByIdAsync(id);
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if(string.IsNullOrEmpty(unformattedKey)) {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            return new AuthenticatorResponse {
                SharedKey = unformattedKey,
                AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey)
            };
        }

        private string GenerateQrCodeUri(string email, string unformattedKey) {
            const string authenticatorUriFormat = "otpauth://totp/{0}?secret={1}";

            return string.Format(authenticatorUriFormat, HttpUtility.UrlEncode($"NetCoreExample.com [{email}]"), unformattedKey);
        }

        public async Task<AuthenticatorStatusResponse> VerifyAndAdd2Fa(long id, AuthenticatorRequest request) {
            var token = request.Token.Replace(" ", string.Empty).Replace("-", string.Empty);

            var user = await _userRepo.GetByIdAsync(id);
            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, token);

            if(isValid) {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
            }

            return new AuthenticatorStatusResponse {
                Success = isValid
            };
        }

        public async Task<bool> Check2FaEnabled(long id) {
            var user = await _userRepo.GetByIdAsync(id);
            return (bool) user.TwoFactorEnabled;
        }

        public async Task<bool?> CheckOptIn(long id){
            var user = await _userRepo.GetByIdAsync(id);
            return (bool?) user.OptedInForEmails;
        }

        public async Task<AuthenticatorStatusResponse> Remove2Fa(long id, AuthenticatorRequest request) {
            var token = request.Token.Replace(" ", string.Empty).Replace("-", string.Empty);

            var user = await _userRepo.GetByIdAsync(id);
            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, token);
            if (isValid) {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
            }

            return new AuthenticatorStatusResponse {
                Success = isValid
            };
        }

        public async Task<AuthenticatorStatusResponse> Remove2FaByAdmin(long userId) {
            var user = await _userRepo.GetByIdAsync(userId);
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            return new AuthenticatorStatusResponse {
                Success = true
            };
        }

        public async Task<bool> UpdateEmail(long userId, string email) {
            var existingUser = await _userRepo.GetByEmailAsync(email);
            if (existingUser != null)
            {
                throw new EmailAlreadyExistsException("Email Already Exists");
            }
            var user = await _userRepo.GetByIdAsync(userId);
            user.Email = email;
            user.NormalizedEmail = _userManager.NormalizeEmail(email);
            user.UserName = email;
            user.NormalizedUserName = _userManager.NormalizeName(user.UserName);
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteUser(long id)
        {
            return await _userRepo.DeleteAsync(id);
        }

        public async Task<string> GetLastLoggedIp(long userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            return user.LastLoggedInIp;
        }

        private string GetEmailPrefix(string email)
        {
            var index = email.IndexOf("@");

            return email.Substring(0, index);
        }

        private string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using var hmacsha256 = new HMACSHA256(keyByte);
            byte[] hashBytes = hmacsha256.ComputeHash(messageBytes);
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("x2"));
            string hexString = sb.ToString();
            return hexString;
        }

    }
}
