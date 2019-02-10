using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Buiness.Hepler
{
    public class UserHelper:IUserBll
    {
        private readonly UserManager<UserEntity> _userManager;

        public UserHelper(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Tuple<int,List<UserEntity>>> GetUserList(int pageIndex, int pageSize,string keyword)
        {
            var total = await _userManager.Users.CountAsync(x =>string.IsNullOrEmpty(keyword)||x.UserName.Contains(keyword));
            var data= await _userManager.Users.Where(x=> string.IsNullOrEmpty(keyword) || x.UserName.Contains(keyword)).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new Tuple<int, List<UserEntity>>(total, data);
        }

        public async Task<UserEntity> GetUserById(Guid id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<IdentityResult> DeleteUserById(Guid id)
        {
            var user= await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> AddUserRoleAsync(UserEntity user, IEnumerable<string> roles)
        {
            return await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task<IdentityResult> DeleteUserRoleAsync(UserEntity user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<bool> ExistUserByCondition(Expression<Func<UserEntity,bool>> expression)
        {
            return await _userManager.Users.AnyAsync(expression);
        }

        public async Task<IdentityResult> AddUser(UserEntity entity,string password)
        {
            return await _userManager.CreateAsync(entity, password);
        }
    }
}
