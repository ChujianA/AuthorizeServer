using AuthorizeServer.Helper;
using DataAccess.DbContext;
using DataAccess.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace AuthorizeServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        public static readonly string MigrationAssembly = typeof(PersistantDbContext).GetTypeInfo().Assembly.GetName().Name;


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            string connectStr = Configuration.GetConnectionString("DbStr");
            services.AddDbContext<PersistantDbContext>(option =>
               {
                   option.UseSqlServer(connectStr,
                       sql => sql.MigrationsAssembly(MigrationAssembly));
               });
            services.AddIdentity<UserEntity, RoleEntity>(options =>
                {
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_-!?@#";
                    options.User.RequireUniqueEmail = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.AllowedForNewUsers = true;
                })
                .AddEntityFrameworkStores<PersistantDbContext>()
                .AddDefaultTokenProviders();

            //AddConfigurationStore 配置用EF实现IdentityServer的客户存储、资源存储、CorsPolicyService（CORS代理服务）
            services.AddIdentityServer(option => { option.UserInteraction.LoginUrl = "/Account/Login"; })
                .AddConfigurationStore<PersistantDbContext>(options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(connectStr, x => x.MigrationsAssembly(MigrationAssembly));
                }).AddOperationalStore<PersistantDbContext>(options =>
                {
                    options.ConfigureDbContext = b =>
                    {
                        b.UseSqlServer(connectStr,
                            sql => sql.MigrationsAssembly(MigrationAssembly));
                    };
                    //自动令牌清理. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                }).AddAspNetIdentity<UserEntity>().AddDeveloperSigningCredential();

            services.AddExternalIdentityProviders();
            services.AddAutoMapper();
            services.AddServices();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
                // app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseIdentityServer();
           

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
