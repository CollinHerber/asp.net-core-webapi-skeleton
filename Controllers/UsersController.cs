using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interfaces;
using WebApi.Models;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(User userParam)
        {
            var user = await _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create (User user)
        {
            await _userService.Create(user);

            return Ok(new { message = "User Created" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            if(users.Count <= 0)
            {
                return Ok(new { message = "No Users Found" });
            }
            return Ok(users);
        }

        [HttpGet("getbyname")]
        public async Task<IActionResult> GetByName(string name)
        {
            var users = await _userService.GetByName(name);
            if (users.Count <= 0)
            {
                return Ok(new { message = "No Users Found" });
            }
            return Ok(users);
        }
    }
}