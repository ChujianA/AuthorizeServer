using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models
{
    public class UserEntity:IdentityUser<Guid>
    {
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
        public virtual ICollection<UserClaimEntity> UserClaims { get; set; }
        public virtual ICollection<UserTokenEntity> UserTokens { get; set; }
        public virtual ICollection<UserLoginEntity> UserLogins { get; set; }
    }
}
