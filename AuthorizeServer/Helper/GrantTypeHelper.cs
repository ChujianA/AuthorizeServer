using System.Collections.Generic;
using System.Linq;
using AuthorizeServer.ViewModels.Enums;

namespace AuthorizeServer.Helper
{
    public class GrantTypeHelper
    {
        public static ICollection<string> GetGrantType(GrantType type)
        {
            switch (type)
            {
                case GrantType.AuthorizationCode:
                    {
                        return IdentityServer4.Models.GrantTypes.Code;
                    }
                case GrantType.Hybrid:
                    {
                        return IdentityServer4.Models.GrantTypes.Hybrid;
                    }
                case GrantType.Implicit:
                    {
                        return IdentityServer4.Models.GrantTypes.Implicit;
                    }
                case GrantType.ClientCredentials:
                    {
                        return IdentityServer4.Models.GrantTypes.ClientCredentials;
                    }
                case GrantType.DeviceFlow:
                    {
                        return IdentityServer4.Models.GrantTypes.DeviceFlow;
                    }
                case GrantType.ResourceOwnerPassword:
                    {
                        return IdentityServer4.Models.GrantTypes.ResourceOwnerPassword;
                    }
                case GrantType.CodeAndClientCredentials:
                    {
                        return IdentityServer4.Models.GrantTypes.CodeAndClientCredentials;
                    }
                case GrantType.HybridAndClientCredentials:
                    {
                        return IdentityServer4.Models.GrantTypes.HybridAndClientCredentials;
                    }
                case GrantType.ImplicitAndClientCredentials:
                    {
                        return IdentityServer4.Models.GrantTypes.ImplicitAndClientCredentials;
                    }
                case GrantType.ResourceOwnerPasswordAndClientCredentials:
                    {
                        return IdentityServer4.Models.GrantTypes.ResourceOwnerPasswordAndClientCredentials;
                    }
                    default:
                        return new List<string>();
            }

        }

    }
}
