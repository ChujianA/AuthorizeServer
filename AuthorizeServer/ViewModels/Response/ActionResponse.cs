using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeServer.ViewModels.Response
{
    public class ActionResponse
    {
        public string ErrorMessage { get; set; }
        public bool Successed { get; set; } = false;
    }
}
