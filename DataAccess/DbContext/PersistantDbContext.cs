using DataAccess.Models;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace DataAccess.DbContext
{
    public class PersistantDbContext:IdentityDbContext<UserEntity,RoleEntity,Guid,UserClaimEntity, UserRoleEntity, UserLoginEntity,RoleClaimEntity,UserTokenEntity>, IConfigurationDbContext, IPersistedGrantDbContext
    {
        private readonly ConfigurationStoreOptions _storeOptions;
        private readonly OperationalStoreOptions _operationalStoreOptions;
        public PersistantDbContext(DbContextOptions<PersistantDbContext> options, ConfigurationStoreOptions storeOptions, OperationalStoreOptions operationalStoreOptions) :base(options)
        {
            _storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions)); ;
            _operationalStoreOptions =operationalStoreOptions??throw  new ArgumentNullException(nameof(operationalStoreOptions));
        }
     

        public DbSet<UserEntity> UserEntities { get; set; }
        public DbSet<RoleEntity> RoleEntities { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

       

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ConfigureClientContext(_storeOptions);
            builder.ConfigureResourcesContext(_storeOptions);
            builder.ConfigurePersistedGrantContext(_operationalStoreOptions);
            base.OnModelCreating(builder);
            builder.Entity<UserEntity>(b =>
                {
                    b.HasMany(e => e.UserClaims).WithOne(e=>e.User).HasForeignKey(c => c.UserId).IsRequired();
                    b.HasMany(e => e.UserTokens).WithOne(e => e.User).HasForeignKey(t => t.UserId).IsRequired();
                    b.HasMany(e => e.UserLogins).WithOne(e => e.User).HasForeignKey(l => l.UserId).IsRequired();
                    b.HasMany(e => e.UserRoles)
                        .WithOne(e => e.User)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
                    b.ToTable("UserInfo");
                });
            builder.Entity<UserClaimEntity>(b => { b.ToTable("UserClaim"); });
            builder.Entity<UserTokenEntity>(b => { b.ToTable("UserToken"); });
            builder.Entity<UserLoginEntity>(b => { b.ToTable("UserLogin"); });
            builder.Entity<UserRoleEntity>(b =>
            {
                b.HasKey(x => new {x.UserId,x.RoleId });
                b.ToTable("UserRole");
            });
            builder.Entity<RoleEntity>(b =>
            {
                b.HasMany(e => e.UserRoles).WithOne(e => e.Role).HasForeignKey(x => x.RoleId).IsRequired();
                b.HasMany(e => e.RoleClaims).WithOne(e => e.Role).HasForeignKey(x => x.RoleId).IsRequired();
                b.ToTable("RoleInfo");
            });
            builder.Entity<RoleClaimEntity>(b => { b.ToTable("RoleClaim"); });
        }
    }
}
