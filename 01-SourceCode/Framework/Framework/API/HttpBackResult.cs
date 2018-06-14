using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class HttpBackResult<T>
    {
        public int Status { get; set; }

        public string Msg { get; set; }

        public T Data { get; set; }
    }
}
