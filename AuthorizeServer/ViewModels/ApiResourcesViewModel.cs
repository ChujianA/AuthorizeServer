using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizeServer.Models;

namespace AuthorizeServer.ViewModels
{
    public class ApiResourcesViewModel
    {
        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool NonEditable { get; set; }
        public ICollection<string> UserClaims { get; set; } = new HashSet<string>();
        public ICollection<SecretViewModel> ApiSecrets { get; set; } = new HashSet<SecretViewModel>();
        public ICollection<ScopeViewModel> Scopes { get; set; } = new HashSet<ScopeViewModel>();
    }
}
