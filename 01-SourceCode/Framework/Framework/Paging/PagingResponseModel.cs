using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class PagingResponseModel<T> : PagingModel where T : class
    {
        public List<T> List { get; set; }

        public int Total { get; set; }

        public int TotalPage
        {
            get
            {
                if (PageSize == 0)
                {
                    return 0;
                }
                else
                {
                    return Total % PageSize == 0 ? Total / PageSize : Total / PageSize + 1;
                }
            }
        }
    }
}
