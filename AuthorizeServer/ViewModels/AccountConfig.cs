using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeServer.ViewModels
{
    //账户配置
    public class AccountConfig
    {
        public static bool AllowRememberLogin = true;
        public static bool AllowLocalLogin = true;
        public static readonly string WindowsAuthenticationSchemeName = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
    }
}
