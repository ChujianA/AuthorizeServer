using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeServer.ViewModels
{
    public class RoleViewModel
    {
        public  Guid Id { get; set; }
        public  string Name { get; set; }
        
        public  string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }
}
