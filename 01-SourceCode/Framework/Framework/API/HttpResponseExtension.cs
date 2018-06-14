using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public static class HttpResponseExtension
    {
        public static HttpResponseResult ConvertToResult(this HttpResponseMessage response)
        {
            HttpResponseResult result;
            try
            {
                response.EnsureSuccessStatusCode();
                result = Task.Run(async () => await response.Content.ReadAsStringAsync()).ContinueWith(m => new HttpResponseResult(m.Result, HttpStatusCode.OK)).Result;
            }
            catch (HttpRequestException ex)
            {
                result = new HttpResponseResult(ex.Message, HttpStatusCode.InternalServerError);
            }
            return result;
        }
    }
}
