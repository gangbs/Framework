using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
   public class BaseApiController
    {
        protected HttpResponseMessage CreateResponseMessage(HttpBackCode code)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var back = new HttpBackResult<string> { Status = (int)code, Msg = code.GetDescribeInfo(), Data = null };
            response.Content = new StringContent(back.ToJson());
            return response;
        }

        protected HttpResponseMessage CreateResponseMessage(string data)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var back = new HttpBackResult<string> { Status = (int)HttpBackCode.Success, Msg = HttpBackCode.Success.GetDescribeInfo(), Data = data };
            response.Content = new StringContent(back.ToJson());
            return response;
        }

        protected HttpResponseMessage CreateResponseMessage<T>(T data)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var back = new HttpBackResult<T> { Status = (int)HttpBackCode.Success, Msg = HttpBackCode.Success.GetDescribeInfo(), Data = data };
            response.Content = new StringContent(back.ToJson());
            return response;
        }

        protected HttpResponseMessage CreateResponseMessage(HttpBackCode code, string data, string message = null)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var back = new HttpBackResult<string> { Status = (int)code, Msg = message == null ? code.GetDescribeInfo() : message, Data = data };
            response.Content = new StringContent(back.ToJson());
            return response;
        }

        protected HttpResponseMessage CreateResponseMessage<T>(HttpBackCode code, T data, string message = null)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var back = new HttpBackResult<T> { Status = (int)code, Msg = message == null ? code.GetDescribeInfo() : message, Data = data };
            response.Content = new StringContent(back.ToJson());
            return response;
        }

        protected HttpResponseMessage CreateRedirectMessage(string url)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(url);
            return response;
        }
    }
}
