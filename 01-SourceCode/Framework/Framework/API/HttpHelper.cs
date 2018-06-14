using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class HttpHelper
    {
        public HttpHelper()
        {
        }

        private HttpClient InitHttp()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//返回json,可以不写这一段，因为默认就是返回json
            httpClient.DefaultRequestHeaders.Add("KeepAlive", "false");   // HTTP KeepAlive设为false，防止HTTP连接保持
            httpClient.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11");
            return httpClient;
        }

        public void SetHead(Dictionary<string, string> dicHead)
        {

        }

        public async Task<HttpResponseResult> Get(string url)
        {
            HttpResponseResult result = null;

            using (var http = this.InitHttp())
            {
                var response = await http.GetAsync(url);
                result = response.ConvertToResult();
            }

            return result;
        }

        public async Task<HttpResponseResult> Get(string url, object param)
        {
            url = this.GetUrl(url, param);
            HttpResponseResult result = null;
            using (var http = this.InitHttp())
            {
                var response = await http.GetAsync(url);
                result = response.ConvertToResult();
            }
            return result;
        }

        public async Task<T> Get<T>(string url)
        {
            T result;
            using (var http = this.InitHttp())
            {
                var response = await http.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    result = default(T);
                }
            }
            return result;
        }

        public async Task<HttpResponseResult> Post<T>(string url, T data)
        {
            HttpResponseResult result = null;
            if (data == null)
            {
                result = null;
            }

            using (var http = this.InitHttp())
            {
                string json = data.ToJson<T>();
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await http.PostAsync(url, content);
                result = response.ConvertToResult();
            }

            return result;
        }

        public async Task<HttpResponseResult> Post(string url, string json)
        {
            HttpResponseResult result = null;
            if (string.IsNullOrWhiteSpace(json))
            {
                result = null;
            }

            using (var http = this.InitHttp())
            {
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await http.PostAsync(url, content);
                result = response.ConvertToResult();
            }
            return result;
        }

        public async Task<TReturn> Post<TReturn, TData>(string url, TData data)
        {
            TReturn result;
            using (var httpClient = this.InitHttp())
            {
                HttpContent content = new ObjectContent<TData>(data, new JsonMediaTypeFormatter());
                var response = await httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<TReturn>();
                }
                else
                {
                    result = default(TReturn);
                }
            }
            return result;
        }

        public async Task<HttpResponseResult> Delete(string url)
        {
            HttpResponseResult result = null;
            using (var http = this.InitHttp())
            {
                var response = await http.DeleteAsync(url);
                result = response.ConvertToResult();
            }
            return result;
        }

        public async Task<HttpResponseResult> Put(string url, string json)
        {
            HttpResponseResult result = null;
            using (var http = this.InitHttp())
            {
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await http.PutAsync(url, content);
                result = response.ConvertToResult();
            }
            return result;
        }

        public string GetUrl(string url, object o)
        {
            var d = new DynamicHandlerCompiler<object>(o.GetType());
            var values = from t in o.GetType().GetProperties()
                         let value = d.CreaterGetPropertyHandler<dynamic>(t.Name)(o)
                         select $"{t.Name}={value}";
            var a = new Uri(string.Format("{0}?{1}", url, string.Join("&", values)));
            return a.ToString();
        }

    }
}
