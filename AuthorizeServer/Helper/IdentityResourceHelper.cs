using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizeServer.ViewModels;
using IdentityServer4.Models;

namespace AuthorizeServer.Helper
{
    public class IdentityResourceHelper
    {
        public static IdentityResource IdentityResource(IdentityResourcesesViewModel model)
        {
                switch (model.Name)
                {
                    case "openid":
                        return new IdentityResources.OpenId();
                    case "profile":
                        return new IdentityResources.Profile();
                    case "email":
                        return new IdentityResources.Email();
                    case "address":
                        return new IdentityResources.Address();
                    case "phone":
                        return new IdentityResources.Phone();
                    default:
                        return new IdentityResource(model.Name, model.DisplayName, model.UserClaims);
                }
        }
    }
}
