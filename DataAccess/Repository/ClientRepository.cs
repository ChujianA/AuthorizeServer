using IdentityServer4.EntityFramework.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAccess.IRepositorys;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using DataAccess.DbContext;

namespace DataAccess.Repository
{
    public class ClientRepository:IClientRepository
    {
        private readonly PersistantDbContext _persistantDbContext;
        public ClientRepository(PersistantDbContext persistantDbContext)
        {
            _persistantDbContext = persistantDbContext;
        }

        public async Task<Client> GetClientByClientId(string clientId)
        {
           return  await _persistantDbContext.Clients.FirstOrDefaultAsync(x=>x.ClientId==clientId);
        }
    }
}
