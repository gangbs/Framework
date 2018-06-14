using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class PagingModel
    {
        public PagingModel()
        {
            this.PageIndex = 1;
            this.PageSize = 10;
        }

        [Ignore]
        public int PageIndex { get; set; }

        [Ignore]
        public int PageSize { get; set; }
    }
}
