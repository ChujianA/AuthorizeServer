﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models
{
    public class UserLoginEntity:IdentityUserLogin<Guid>
    {
        public virtual UserEntity User { get; set; }
    }
}
