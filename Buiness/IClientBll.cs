using IdentityServer4.Models;
using System.Threading.Tasks;
using entity = IdentityServer4.EntityFramework.Entities;

namespace Buiness
{
    public interface IClientBll
    {
        Task<Client> GetEnabledClientByClientId(string clientId);

        Task<bool> IsPkceClientAsync(string clientId);
        /// <summary>
        /// 添加客户端信息
        /// </summary>
        /// <param name="clientModel">客户端信息</param>
        /// <returns></returns>
        Task<int> AddClient(Client clientModel);
        /// <summary>
        /// 删除客户端
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<int> DeleteClient(int id);

        Task<entity.Client> GetClientByName(string clientName);
    }
}
