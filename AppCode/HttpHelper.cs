namespace SamweiSpiderApp2021.AppCode
{
    #region _Spaces
    using System.Net;
    using System.Text;
    #endregion

    /// <summary>
    /// HTTP请求类
    /// </summary>
    public class HttpHelper
    {
        #region POST V2
        /// <summary>
        /// 发起POST同步请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="body">POST提交的内容</param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded等</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static bool HttpPost(string? url, out int StateCode, out string HttpMessage, out Dictionary<string, string> OutHeaders, string? body = null, string? contentType = null, Dictionary<string, string>? headers = null, int timeOut = 30)
        {
            StateCode = -1;
            OutHeaders = new Dictionary<string, string>();

            try
            {
                body ??= "";
                using var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 0, timeOut);

                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                using var httpContent = new StringContent(body, Encoding.UTF8);
                if (contentType != null)
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                var response = client.PostAsync(url, httpContent).Result;
                StateCode = (int)response.StatusCode;
                HttpMessage = response.Content.ReadAsStringAsync().Result;

                foreach (var item in client.DefaultRequestHeaders)
                    OutHeaders.Add(item.Key, item.Value.FirstOrDefault());

                return true;
            }
            catch(Exception ex)
            {
                HttpMessage = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// 发起POST异步请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="body">POST提交的内容</param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static async Task<string> HttpPostAsync(string url, string? body = null, string?contentType = null, Dictionary<string, string>? headers = null, int timeOut = 30)
        {
            body ??= "";

            using var client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, timeOut);
            if (headers != null)
            {
                foreach (var header in headers)
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            using var httpContent = new StringContent(body, Encoding.UTF8);
            if (contentType != null)
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            var response = await client.PostAsync(url, httpContent);
            return await response.Content.ReadAsStringAsync();
        }
        #endregion

        #region GET V2
        /// <summary>
        /// 发起GET同步请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static bool HttpGet(string? url, out int StateCode, out string HttpMessage, out Dictionary<string, string> OutHeaders, Dictionary<string, string>? headers = null, int Timeout = 30)
        {
            StateCode = -1;
            OutHeaders = new Dictionary<string, string>();

            try
            {
                using var client = GetHttpClient(headers);
                client.Timeout = new TimeSpan(0, 0, Timeout);

                var response = client.GetAsync(url).Result;
                StateCode = (int)response.StatusCode;
                HttpMessage = response.Content.ReadAsStringAsync().Result;

                foreach (var item in client.DefaultRequestHeaders)
                    OutHeaders.Add(item.Key, item.Value.FirstOrDefault());

                return true;
            }
            catch (Exception ex)
            {
                HttpMessage = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// 发起GET异步请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="headers">请求头</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string url, Dictionary<string, string>? headers = null, int timeOut = 30)
        {
            using var client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, timeOut);
            if (headers != null)
            {
                foreach (var header in headers)
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
        #endregion

        #region HttpClient/Headers
        /// <summary>
        /// HttpClient-Headers
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        private static HttpClient GetHttpClient(Dictionary<string, string>? headers)
        {
            var client = new HttpClient();
            if (headers != null)
            {
                foreach (var header in headers)
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
            client.DefaultRequestHeaders.Add("DNT", "1");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4613.0 Safari/537.36 Edg/95.0.1000.0");

            return client;
        }
        #endregion
    }
}
