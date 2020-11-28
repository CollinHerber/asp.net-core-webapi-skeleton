using NetCoreExample.Server.Data.Interfaces;
using NetCoreExample.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreExample.Server.Data.Repositories {
    public class UserRepository : IUserRepository {
        private readonly UserManager<User> _userManager;
        private readonly NetCoreExampleDbContext _db;

        public UserRepository(UserManager<User> userManager, NetCoreExampleDbContext db) {
            _userManager = userManager;
            _db = db;
        }

        public async Task AddRoleAsync(long userId, string role) {
            var user = await GetByIdAsync(userId);
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IEnumerable<string>> GetRolesAsync(long userId) {
            return await _userManager.GetRolesAsync(await GetByIdAsync(userId));
        }

        public async Task<List<User>> AllAsync() {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(long id) {
            return await _userManager.Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByIdAsNoTracking(long id) {
            return await _userManager.Users.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<int> CountByPhoneNumberAndVerified(string phoneNumber, int countryCode) {
            return await _userManager.Users
                .Where(x => x.PhoneNumber == phoneNumber && x.PhoneCountryCode == countryCode && x.PhoneNumberConfirmed == true)
                .CountAsync();
        }

        public async Task<int> CountByCharacterNameAsync(string characterName) {
            return await _userManager.Users.Where(x => x.CharacterName == characterName).CountAsync();
        }

        public async Task<User> GetByEmailAsync(string email) {
            return await _userManager.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<long> CreateAsync(User user, string password) {
            user.CreatedDate = DateTime.UtcNow;
            user.UserName = user.Email;
            user.Verified = false;
            user.PhoneNumberConfirmed = false;
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) {
                return 0;
                //throw new EntityCreationException(string.Join("\n", result.Errors.Select(e => e.Description)));
            }

            await _userManager.SetLockoutEnabledAsync(user, false);

            return user.Id;
        }

        public async Task UpdateAsync(User user, string[] roles = null) {
            var currentUser = await GetByIdAsync(user.Id);

            _db.Entry(currentUser).CurrentValues.SetValues(user);
            
            var type = typeof(User);
            
            var properties = type.GetProperties();
            foreach (var property in properties) {
                var propValue = property.GetValue(user, null);
                if (!new List<string> { "ConcurrencyStamp", "Password"}.Contains(property.Name) && propValue == null) {
                    _db.Entry(currentUser).Property(property.Name).IsModified = false;
                }
            }
            
            _db.Entry(currentUser).Property(x => x.ConcurrencyStamp).IsModified = false;
            
            
            var result = await _userManager.UpdateAsync(currentUser);
            if (!result.Succeeded) {
                return;
                //throw new EntityCreationException(string.Join("\n", result.Errors));
            }
            
            if (roles != null && roles.Any()) {
                await _userManager.RemoveFromRolesAsync(currentUser, await _userManager.GetRolesAsync(currentUser));
                await _userManager.AddToRolesAsync(currentUser, roles);
            }
            
            await _userManager.UpdateNormalizedEmailAsync(currentUser);
            await _userManager.UpdateNormalizedUserNameAsync(currentUser);
        }

        public async Task<bool> ResetPasswordAsync(string email, string password, string token) {
            var user = await GetByEmailAsync(email);

            foreach (var validator in _userManager.PasswordValidators) {
                var validateCheck = await validator.ValidateAsync(_userManager, user, password);
                if (!validateCheck.Succeeded) {
                    return false;
                }
            }
            var resetStatus = await _userManager.ResetPasswordAsync(user, token, password);
            if (!resetStatus.Succeeded)
                return false;

            await _userManager.SetLockoutEnabledAsync(user, false);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddDays(-1));

            return true;
        }

        public async Task UpdatePasswordAsync(long id, string password) {
            var user = await GetByIdAsync(id);

            foreach (var validator in _userManager.PasswordValidators) {
                var validateCheck = await validator.ValidateAsync(_userManager, user, password);
                if (!validateCheck.Succeeded) {
                    return;
                    //throw new InvalidEntityException(string.Join("\n", validateCheck.Errors.Select(x => x.Description)));
                }
            }

            await _userManager.UpdateSecurityStampAsync(user);
            user.Password = null;
            user.UserName = user.Email;
            await UpdateAsync(user);

            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, password);
            await _userManager.SetLockoutEnabledAsync(user, false);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddDays(-1));
        }

        public async Task<string> GeneratePasswordResetTokenAsync(long id) {
            var user = await GetByIdAsync(id);
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<List<User>> BulkImportEmails(List<User> newUsers)
        {
            var foundUsers = await _userManager.Users.Where(x => newUsers.Select(u => u.Email.ToUpper()).Contains(x.NormalizedEmail)).Select(x => x.NormalizedEmail).ToListAsync();
            var usersToInsert = newUsers.Where(x => !foundUsers.Contains(x.Email.ToUpper())).ToList();
            foreach (var user in usersToInsert)
            {
                user.CreatedDate = DateTime.UtcNow;
                user.UserName = user.Email;
                user.NormalizedEmail = _userManager.NormalizeEmail(user.Email);
                user.NormalizedUserName = _userManager.NormalizeName(user.Email);
                user.EmailConfirmed = false;
                user.AccessFailedCount = 0;
                user.PhoneNumberConfirmed = false;
                await _db.AddAsync(user);
            }

            await _db.SaveChangesAsync();
            return newUsers;
        }

        public virtual async Task<bool> DeleteAsync(long id) {
            var entity = await GetByIdAsync(id);
            if (entity == null || entity.IsDeleted)
            {
                return false;
                //throw new EntityNotFoundException($"{EntityName} not found with the id: {id}");
            }

            entity.IsDeleted = true;
            await UpdateAsync(entity);
            return true;
        }
    }
}
