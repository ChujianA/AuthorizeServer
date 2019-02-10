using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;

namespace Buiness
{
    public  interface IApiResourcesBll
    {
        Task<IEnumerable<ApiResource>> FindApiResourceByScopeAsync(IEnumerable<string> scopeNames);
        
        Task<int> AddApiResourceAsync(ApiResource entity);
        Task<int> DeleteApiResource(int id);
    }
}
