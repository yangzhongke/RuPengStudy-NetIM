using RuPeng.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UserCenter.NetSDK
{
    public class SDKClient
    {
        private String appKey;
        private String appSecret;
        private string serverRoot;

        public SDKClient(string appKey, string appSecret, string serverRoot)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
            this.serverRoot = serverRoot;
        }

        public async Task<SDKResult> ExecuteGetAsync(string url, Dictionary<string, object> data)
        {
            using (HttpClientHandler handler = new HttpClientHandler { UseProxy = false })
            using (HttpClient httpClient = new HttpClient(handler))
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);

                StringBuilder sbQueryString = new StringBuilder();
                var keys = data.Keys.OrderBy(k => k).ToArray();
                for (int i = 0; i < keys.Length; i++)
                {
                    var key = keys[i];
                    var value = data[key];
                    sbQueryString.Append(Uri.EscapeDataString(key)).Append("=")
                        .Append(Uri.EscapeDataString(Convert.ToString(value)));
                    if (i < data.Keys.Count - 1)
                    {
                        sbQueryString.Append("&");
                    }
                }
                string sign = MD5Helper.ComputeMd5(sbQueryString + appSecret);

                using (HttpRequestMessage reqMsg = new HttpRequestMessage(HttpMethod.Get,
                    serverRoot + url + "?" + sbQueryString))
                {
                    reqMsg.Headers.Add("AppKey", appKey);
                    reqMsg.Headers.Add("Sign", sign);

                    using (var resultTask = await httpClient.SendAsync(reqMsg))
                    {
                        SDKResult sdkResult = new SDKResult();
                        sdkResult.Result = await resultTask.Content.ReadAsStringAsync();
                        sdkResult.StatusCode = resultTask.StatusCode;
                        return sdkResult;
                    }
                }
            }
        }
    }
}
