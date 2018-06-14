using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class HttpResponseResult
    {
        readonly string message;
        readonly HttpStatusCode status;
        public HttpStatusCode Status => status;
        public string Message => message;

        public HttpResponseResult(string msg, HttpStatusCode status)
        {
            this.message = msg;
            this.status = status;
        }

        public HttpBackResult<T> GetHttpBackResult<T>()
        {
            if (status == HttpStatusCode.OK)
            {
                return message.ToObject<HttpBackResult<T>>();
            }
            else
            {
                return new HttpBackResult<T> { Data = default(T), Msg = message, Status = (int)HttpBackCode.Http };
            }
        }
    }
}
