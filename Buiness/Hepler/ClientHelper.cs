using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.IRepositorys;
using IdentityServer4.Models;

namespace Buiness.Hepler
{
    public class ClientHelper : IClientBll
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _iMapper;

        public ClientHelper(IClientRepository clientRepository, IMapper iMapper)
        {
            _clientRepository = clientRepository;
            _iMapper = iMapper;
        }
        /// <summary>
        /// 根据客户ID获取可用客户实体
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<Client> GetClientByClientId(string clientId)
        {
            if(string.IsNullOrEmpty(clientId)) throw  new ArgumentNullException(nameof(clientId));
            var client= await _clientRepository.GetClientByClientId(clientId);
            if(client!=null&&client.Enabled)
                return _iMapper.Map<IdentityServer4.EntityFramework.Entities.Client, Client>(client);
            return  new Client();
        }

        public async Task<bool> IsPkceClientAsync(string clientId)
        {
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                var client = await GetClientByClientId(clientId);
                return client?.RequirePkce == true;
            }
            return false;
        }
    }
}
