using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DbContext;
using DataAccess.IRepositorys;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;

namespace DataAccess.Repository
{
    public class ApiResourcesRepository: IApiResourcesRepository
    {
        private readonly PersistantDbContext _persistantDbContext;
        public ApiResourcesRepository(PersistantDbContext persistantDbContext)
        {
            _persistantDbContext = persistantDbContext;
        }

        public Task<IEnumerable<ApiResource>> FindApiResourceByScopeAsync(IEnumerable<string> scopeNames)
        {
            if(scopeNames==null) throw  new ArgumentNullException(nameof(scopeNames));
            var apiResources= _persistantDbContext.ApiResources.Where(x =>x.Scopes.Select(scope=>scopeNames.Contains(scope.Name)).Any()).AsEnumerable();
            return Task.FromResult(apiResources);
        }
    }
}
