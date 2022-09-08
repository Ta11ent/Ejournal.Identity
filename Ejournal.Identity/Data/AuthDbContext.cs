using Ejournal.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ejournal.Identity.Data
{
    public class AuthDbContext : IdentityDbContext<AppUser, AppRole, Guid>    {
        public DbSet<AppUser> AppUsers { get; set; }
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>(entity => entity.ToTable(name: "Users"));
            builder.Entity<AppRole>(entity => entity.ToTable(name: "Roles"));
            builder.Entity<IdentityUserRole<Guid>>(entity => entity.ToTable(name: "UserRoles"));
            builder.Entity<IdentityUserClaim<Guid>>(entity => entity.ToTable(name: "UserClaim"));
            builder.Entity<IdentityUserLogin<Guid>>(entity => entity.ToTable(name: "UserLogins"));
            builder.Entity<IdentityUserToken<Guid>>(entity => entity.ToTable(name: "UserTokens"));
            builder.Entity<IdentityRoleClaim<Guid>>(entity => entity.ToTable(name: "RoleClaims"));

            builder.ApplyConfiguration(new AppUserConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
