using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeServer.ViewModels.Request
{
    public class KeywordPageQueryRequest:PageQueryBaseRequest
    {
        public string Key { get; set; }
    }
}
