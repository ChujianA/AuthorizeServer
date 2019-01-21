using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;

namespace DataAccess.IRepositorys
{
   public interface IClientRepository
   {
       Task<Client> GetClientByClientId(string clientId);
   }
}
