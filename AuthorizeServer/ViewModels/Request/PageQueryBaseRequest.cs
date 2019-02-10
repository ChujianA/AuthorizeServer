using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizeServer.ViewModels.Request
{
    public class PageQueryBaseRequest
    {
        private  int _pageIndex;
        private  int _pageSize;
        public int PageIndex { get => _pageIndex != 0 ? _pageIndex : 1; set => _pageIndex = value; }

        public int PageSize { get => _pageSize != 0 ? _pageSize : 10; set => _pageSize = value; }

    }
}
