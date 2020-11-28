using System.Collections.Generic;
using System.Threading.Tasks;
using NetCoreExample.Server.Models;
using NetCoreExample.Server.Models.DTOs.User.Request;
using NetCoreExample.Server.Models.DTOs.User.Response;
using DevExtreme.AspNet.Data.ResponseModel;

namespace NetCoreExample.Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> Authenticate(string username, string password, string token, string ip, bool isAdmin = false);
        Task<LoginResponse> CreateUser(RegisterUserDto user);
        Task<LoginResponse> CreateUserByEmail(RegisterUserDto userData);
        Task<User> UpdateUser(UserEmployeeDto user);
        Task<User> GetById(long id);
        Task<bool> CheckEmail(string email);
        Task<User> FindByEmail(RequestResetPasswordRequest data);
        Task<bool> SendResetPasswordEmail(RequestResetPasswordRequest data);
        Task<User> UpdateInformationById(long id, UpdateInformationRequest data);
        Task<bool> UpdatePassword(long id, UpdatePasswordRequest resetPasswordRequest);
        Task<AuthenticatorResponse> GenerateTotpData(long id);
        Task<AuthenticatorStatusResponse> VerifyAndAdd2Fa(long id, AuthenticatorRequest request);
        Task<bool> Check2FaEnabled(long id);
        Task<AuthenticatorStatusResponse> Remove2Fa(long id, AuthenticatorRequest request);
        Task<LoginResponse> RefreshToken(long id);
        Task<bool> ResetPasswordByToken(ResetPasswordRequest request);
        Task<User> GetProfile(long id);
        Task<AuthenticatorStatusResponse> Remove2FaByAdmin(long userId);
        Task<bool> UpdateEmail(long userId, string email);
        Task<bool> UpdatePasswordAdmin(long id, UpdatePasswordRequest resetPasswordRequest);
        Task<bool?> CheckOptIn(long id);
        Task<bool> DeleteUser(long id);
        Task<string> GetLastLoggedIp(long userId);
        void Logout(long userId);
    }
}