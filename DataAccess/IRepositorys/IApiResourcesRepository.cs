using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositorys
{
    public interface IApiResourcesRepository
    {
        /// <summary>
        /// 通过ScopeName获取apiResource
        /// </summary>
        /// <param name="scopeNames"></param>
        /// <returns></returns>
        Task<IEnumerable<ApiResource>> FindApiResourceByScopeAsync(IEnumerable<string> scopeNames);

    }
}
