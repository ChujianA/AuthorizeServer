using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeServer.ViewModels
{
    public class IdentityResourcesesViewModel
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public ICollection<string> UserClaims { get; set; } = new HashSet<string>();
        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
        public bool Required { get; set; } = false;
        public bool Emphasize { get; set; } = false;
        public bool ShowInDiscoveryDocument { get; set; } = true;
    }
}
