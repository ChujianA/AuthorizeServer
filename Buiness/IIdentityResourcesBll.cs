using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;

namespace Buiness
{
    public interface IIdentityResourcesBll
    {
        Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames);
        Task<int> AddIdentityResources(IdentityResource entity);
        Task<int> AddIdentityResourcesList(List<IdentityResource> list);
    }
}
