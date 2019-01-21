using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdModel = IdentityServer4.Models;

namespace Buiness
{
    public interface IResourcesBll
    {
        Task<IdModel.Resources> FindResourcesByScopeAsync(IEnumerable<string> scopeNames);
    }
}
