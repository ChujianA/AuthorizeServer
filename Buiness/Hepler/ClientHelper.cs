using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.DbContext;
using DataAccess.Models;
using IdentityServer4.EntityFramework.Mappers;
using entity = IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;

namespace Buiness.Hepler
{
    public class ClientHelper : IClientBll
    {
        private readonly PersistantDbContext _persistantDbContext;
        private readonly IMapper _iMapper;
        public ClientHelper(PersistantDbContext persistantDbContext, IMapper iMapper)
        {
            _persistantDbContext = persistantDbContext;
            _iMapper = iMapper;
        }
        /// <summary>
        /// 根据客户ID获取可用客户实体
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<Client> GetEnabledClientByClientId(string clientId)
        {
            if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
            var client = await _persistantDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client != null && client.Enabled)
                return _iMapper.Map<entity.Client, Client>(client);
            return new Client();
        }

        public async Task<entity.Client> GetClientByName(string clientName)
        {
            return await _persistantDbContext.Clients.FirstOrDefaultAsync(x => x.ClientName == clientName);
        }

        public async Task<bool> IsPkceClientAsync(string clientId)
        {
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                var client = await _persistantDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
                return client?.RequirePkce == true;
            }
            return false;
        }

        public async Task<int> AddClient(Client client)
        {
            var entity = client.ToEntity();
            await _persistantDbContext.Clients.AddAsync(entity);
            entity.RequireClientSecret = entity.ClientSecrets.Any();
            return await _persistantDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteClient(int id)
        {
            var client = await _persistantDbContext.Clients.FirstOrDefaultAsync(x => x.Id == id);
            if (client == null || client.Id == 0)
            {
                throw new Exception("不存在此客户端");
            }
            _persistantDbContext.Clients.Remove(client);

            return await _persistantDbContext.SaveChangesAsync();
        }
    }
}
