using WebApi.Models;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace WebApi.Interfaces.IUserService
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);

        Task<List<User>> GetAll();

        Task<List<User>> GetByName(string name);

        Task Create(User user);
    }
}
