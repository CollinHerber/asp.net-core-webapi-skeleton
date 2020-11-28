using System.Linq.Expressions;
using NetCoreExample.Server.Data.Configurations;
using NetCoreExample.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreExample.Server.Models;

namespace NetCoreExample.Server.Data
{
    public class NetCoreExampleDbContext : IdentityDbContext<User, IdentityRole<long>, long, IdentityUserClaim<long>, IdentityUserRole<long>, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public NetCoreExampleDbContext(DbContextOptions options) : base(options)
        {
            // no-op
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole<long>>().ToTable("Role");
            modelBuilder.Entity<IdentityUserClaim<long>>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserLogin<long>>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityRoleClaim<long>>().ToTable("RoleClaim");
            modelBuilder.Entity<IdentityUserToken<long>>().ToTable("UserToken");
            modelBuilder.Entity<IdentityUserRole<long>>().ToTable("UserRole");

            modelBuilder.ApplyConfiguration(new UserConfiguration());

            //modelBuilder.Entity<UserNote>().HasQueryFilter(e => !e.IsDeleted);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var isDeletedProperty = entityType.FindProperty("IsDeleted");
                if (isDeletedProperty != null && isDeletedProperty.ClrType == typeof(bool))
                {
                    var parameter = Expression.Parameter(entityType.ClrType);
                    var filter = Expression.Lambda(Expression.Not(Expression.Property(parameter, isDeletedProperty.PropertyInfo)), parameter);
                    entityType.SetQueryFilter(filter);
                }
            }
        }
    }
}
