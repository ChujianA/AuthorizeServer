using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models
{
   public class UserRoleEntity:IdentityUserRole<Guid>
    {
        public virtual UserEntity User{ get; set; }
        public virtual RoleEntity Role { get; set; }
    }
}
