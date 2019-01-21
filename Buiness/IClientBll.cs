using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace Buiness
{
    public interface IClientBll
    {
        Task<Client> GetClientByClientId(string clientId);
        Task<bool> IsPkceClientAsync(string clientId);
    }
}
