using NetCoreExample.Server.Data.Interfaces.Base;
using NetCoreExample.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data.ResponseModel;

namespace NetCoreExample.Server.Data.Interfaces {
    public interface IUserRepository {
        Task<User> GetByIdAsync(long id);
        Task<User> GetByEmailAsync(string email);
        Task<long> CreateAsync(User user, string password);
        Task UpdateAsync(User user, string[] roles = null);
        Task UpdatePasswordAsync(long id, string password);
        Task<string> GeneratePasswordResetTokenAsync(long id);
        Task<bool> ResetPasswordAsync(string email, string password, string token);
        Task<List<User>> AllAsync();
        Task<User> GetByIdAsNoTracking(long id);
        Task<IEnumerable<string>> GetRolesAsync(long userId);
        Task AddRoleAsync(long userId, string role);
        Task<bool> DeleteAsync(long id);
    }
}
