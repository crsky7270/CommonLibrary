using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crsky.DBUtility
{
    public class PagerInfo
    {
        int _page = 1;//当前页
        int _pageSize = 10;
        public int Page { get { return _page; } set { _page = value; } }
        public int PageSize { get { return _pageSize; } set { _pageSize = value; } }
    }
}
