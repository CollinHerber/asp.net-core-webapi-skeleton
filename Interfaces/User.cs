using WebApi.Models;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace WebApi.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);

        Task<string> CreateToken(long userId);

        Task<List<User>> GetAll();

        Task<List<User>> GetByName(string name);

        Task Create(User user);
    }
}
