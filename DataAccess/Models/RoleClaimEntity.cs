using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models
{
   public  class RoleClaimEntity:IdentityRoleClaim<Guid>
    {
        public virtual RoleEntity Role { get; set; }
    }
}
