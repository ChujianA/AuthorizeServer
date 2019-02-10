using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeServer.ViewModels.Response
{
    public class QueryResponse<T> where T:class
    {
        public int Total { get; set; }
        public List<T> data { get; set; }

        public QueryResponse(int total):base()
        {
            Total = total;
        }

        public QueryResponse()
        {

        }

        public int State { get; set; }
    }
}
