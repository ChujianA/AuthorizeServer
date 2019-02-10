using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeServer.Models
{
    public class UserLoginModel
    {
        public bool RememberMe { get; set; }
        [Required]
        [StringLength(32)]
        [MinLength(4)]
        public string UserName { get; set; }

        [StringLength(256)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(24)]
        [MinLength(6)]
        [Required]
        public string Password { get; set; }

        [Phone]
        [StringLength(50)]
        public string PhoneNumber { get; set; }
    }
}
