using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using WebApi.Helpers;
using WebApi.Interfaces;
using System.Threading.Tasks;
using WebApi.Server;

namespace WebApi.Services
{

    public class UserService : IUserService
    {

        private readonly AppSettings _appSettings;
        private readonly WebApiDbContext _dbContext;


        public UserService(WebApiDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
        }

        public async Task<string> CreateToken(long userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            return user;
        }

        public Task Create(User user)
        {
            _dbContext.Users.Add(user);

            return _dbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetAll()
        {
            // return users without passwords
            return _dbContext.Users.ToList();
        }

        public async Task<List<User>> GetByName(string name)
        {
            // return users without passwords
            return _dbContext.Users.Where(item => item.FirstName == name).ToList();
        }
    }
}