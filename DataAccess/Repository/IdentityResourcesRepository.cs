using DataAccess.IRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using DataAccess.DbContext;

namespace DataAccess.Repository
{
    public class IdentityResourcesRepository: IIdentityResourcesRepository
    {
        private readonly PersistantDbContext _persistantDbContext;
        public IdentityResourcesRepository(PersistantDbContext persistantDbContext)
        {
            _persistantDbContext = persistantDbContext;
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if(scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));
            var identityResources = _persistantDbContext.IdentityResources.Where(x => scopeNames.Contains(x.Name))
                .AsEnumerable();
            return Task.FromResult(identityResources);
        }
    }
}
