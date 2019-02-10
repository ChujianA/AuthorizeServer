using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buiness.Hepler
{
    public class RoleHelper:IRoleBll
    {
      
        private readonly RoleManager<RoleEntity> _roleManager;
        public RoleHelper(RoleManager<RoleEntity> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Tuple<int,List<RoleEntity>>> GetRoleList(int pageIndex, int pageSize,string keyword)
        {
           var total =await _roleManager.Roles.CountAsync(x => x.Name.Contains(keyword));
           var data=await _roleManager.Roles.Where(x=>x.Name.Contains(keyword)).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
           return  new Tuple<int, List<RoleEntity>>(total,data);
        }

        public async Task<RoleEntity> FindRoleById(Guid id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<RoleEntity> GetRoleByRoleName(string roleName)
        {
            return await _roleManager.Roles.FirstOrDefaultAsync(x => x.Name == roleName);
        }

        public async Task<IdentityResult> DeleteRole(RoleEntity role)
        {
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<IdentityResult> AddRole(RoleEntity entity)
        {
            return await _roleManager.CreateAsync(entity);
        }
       
    }
}
