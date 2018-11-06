using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Framework
{
   public class MvcBaseController:Controller
    {
        public AjaxWebResponse CreateSuccessResonse()
        {
            var res = new AjaxWebResponse(true);
            return res;
        }

        public AjaxWebResponse CreateSuccessResonse<T>(T data)
        {
            var res = new AjaxWebResponse(true)
            {
                 result=data
            };
            return res;
        }

        public AjaxWebResponse CreateErrorResonse()
        {
            var res = new AjaxWebResponse(false);
            return res;
        }

        public AjaxWebResponse CreateErrorResonse(AjaxErrorWebResponse errorInfo)
        {
            var res = new AjaxWebResponse(false)
            {
                error = errorInfo
            };
            return res;
        }

        public AjaxWebResponse CreateUnAuthorizeResonse(string url=null)
        {
            var res = new AjaxWebResponse(false)
            {
                 unAuthorizedRequest=true,
                 targetUrl=url
            };
            return res;
        }
    }
}
