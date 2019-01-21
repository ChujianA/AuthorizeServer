using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeServer.Models
{
    public class ConsentConfig
    {
        /// <summary>
        /// 启用脱机访问
        /// </summary>
        public static bool EnableOfflineAccess = true;
        public static string OfflineAccessDisplayName = "Offline Access";
        public static string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";

        public static readonly string MustChooseOneErrorMessage = "至少选择一个权限给与授权";
        public static readonly string InvalidSelectionErrorMessage = "Invalid selection";
    }
}
