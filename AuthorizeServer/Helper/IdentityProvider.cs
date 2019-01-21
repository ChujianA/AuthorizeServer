using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizeServer.Helper
{
    public static class IdentityProvider
    {
        public static IServiceCollection AddExternalIdentityProviders(this IServiceCollection services)
        {
            services.AddAuthentication("mycookie")
                .AddCookie("mycookie", option => { option.ExpireTimeSpan = TimeSpan.Parse("3600"); })
                .AddGoogle("Goole", options =>
                    {
                        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                        options.ClientId = "googleuser";
                        options.ClientSecret = "googlesecret";
                    });
            return services;
        }
    }
}
