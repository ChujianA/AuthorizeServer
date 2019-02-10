using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DbContext;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace Buiness.Hepler
{
    public class ApiResourcesHelper:IApiResourcesBll
    {
        private readonly PersistantDbContext _persistantDbContext;
        public ApiResourcesHelper(PersistantDbContext persistantDbContext)
        {
            _persistantDbContext = persistantDbContext;
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourceByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));
            var apiresource= _persistantDbContext.ApiResources.Include(x => x.Scopes).Include(x => x.Secrets).Where(x => x.Scopes.Select(scope => scopeNames.Contains(scope.Name)).Any());
            return await Task.FromResult(apiresource);
        }

        public async Task<int> AddApiResourceAsync(ApiResource entity)
        {
            await _persistantDbContext.ApiResources.AddAsync(entity);
            return await _persistantDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteApiResource(int id)
        {
           var apiResource=await _persistantDbContext.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (apiResource == null) throw new ArgumentNullException("不存在此资源");
            _persistantDbContext.Remove(apiResource);
            return await _persistantDbContext.SaveChangesAsync();
        }

    }
}
