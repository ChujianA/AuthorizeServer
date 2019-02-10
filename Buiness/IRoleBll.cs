using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace Buiness
{
    public interface IRoleBll
    {
        Task<Tuple<int, List<RoleEntity>>> GetRoleList(int pageIndex, int pageSize, string keyword);
        Task<RoleEntity> FindRoleById(Guid id);
       Task<RoleEntity>  GetRoleByRoleName(string roleName);
        Task<IdentityResult> DeleteRole(RoleEntity role);
        Task<IdentityResult> AddRole(RoleEntity entity);

    }
}
