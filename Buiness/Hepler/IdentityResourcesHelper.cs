using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DbContext;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;

namespace Buiness.Hepler
{
    public class IdentityResourcesHelper: IIdentityResourcesBll
    {

        private readonly PersistantDbContext _persistantDbContext;
        public IdentityResourcesHelper(PersistantDbContext persistantDbContext)
        {
            _persistantDbContext = persistantDbContext;
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));
            var identityResources = _persistantDbContext.IdentityResources.Where(x => scopeNames.Contains(x.Name))
                .AsEnumerable();
            return Task.FromResult(identityResources);
        }

        public async Task<int> AddIdentityResources(IdentityResource entity)
        {
            await _persistantDbContext.IdentityResources.AddAsync(entity);
            return await _persistantDbContext.SaveChangesAsync();
        }

        public async Task<int> AddIdentityResourcesList(List<IdentityResource> list)
        {
            foreach (var item in list)
            {
                await _persistantDbContext.IdentityResources.AddAsync(item);
            }
            return await _persistantDbContext.SaveChangesAsync();
        }
    }
}
