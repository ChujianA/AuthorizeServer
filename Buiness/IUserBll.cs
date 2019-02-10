using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace Buiness
{
    public interface IUserBll
    {
        Task<Tuple<int, List<UserEntity>>> GetUserList(int pageIndex, int pageSize, string keyword);
        Task<UserEntity> GetUserById(Guid id);
        Task<IdentityResult> DeleteUserById(Guid id);
        Task<bool> ExistUserByCondition(Expression<Func<UserEntity, bool>> expression);
        Task<IdentityResult> AddUserRoleAsync(UserEntity user, IEnumerable<string> roles);
        Task<IdentityResult> DeleteUserRoleAsync(UserEntity user, string role);
        Task<IdentityResult> AddUser(UserEntity entity, string password);
    }
}
