using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeServer.ViewModels.Enums
{
    public enum GrantType
    {
        Implicit,
        ImplicitAndClientCredentials,
        CodeAndClientCredentials,
        AuthorizationCode,
        Hybrid,
        HybridAndClientCredentials,
        ClientCredentials,
        ResourceOwnerPassword,
        ResourceOwnerPasswordAndClientCredentials,
        DeviceFlow
    }
}
