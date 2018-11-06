using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class AjaxWebResponse
    {
        public AjaxWebResponse(bool isSuccess)
        {
            this.success = isSuccess;
        }

        public bool success { get; set; }

        public object result { get; set; }

        public AjaxErrorWebResponse error { get; set; }

        public string targetUrl { get; set; }

        public bool unAuthorizedRequest { get; set; }
    }


    public class AjaxWebResponse<T> where T : class
    {
        public AjaxWebResponse(bool isSuccess)
        {
            this.success = isSuccess;
        }

        public bool success { get; set; }

        public T result { get; set; }

        public AjaxErrorWebResponse error { get; set; }

        public string targetUrl{get;set;}

        public bool unAuthorizedRequest { get; set; }
    }


    public class AjaxErrorWebResponse
    {
        public string message { get; set; }

        public object detail { get; set; }
    }

}
