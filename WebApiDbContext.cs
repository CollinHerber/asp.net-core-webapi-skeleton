using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Server
{
    public class WebApiDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public WebApiDbContext(DbContextOptions<WebApiDbContext> options) : base(options)
        {

        }
    }
}
