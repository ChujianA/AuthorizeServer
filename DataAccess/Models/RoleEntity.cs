using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models
{
    public class RoleEntity:IdentityRole<Guid>
    {
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
        public virtual ICollection<RoleClaimEntity> RoleClaims { get; set; }
    }
}
