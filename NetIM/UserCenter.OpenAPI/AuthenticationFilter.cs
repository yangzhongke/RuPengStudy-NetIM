using RuPeng.Common;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using UserCenter.IServices;

namespace UserCenter.OpenAPI
{
    //不要System.Web.Mvc下的IAuthorizationFilter
    //而是用System.Web.Http.Filters下的
    //然后在WebApiConfig中配置：config.Filters.Add(new AuthenticationFilter());
    public class AuthenticationFilter : IAuthorizationFilter
    {
        //public IAppInfoService AppInfoService { get; set; }

        public bool AllowMultiple { get { return true; } }

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, 
            CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            //由于AuthenticationFilter是在WebApiConfig中直接new的，所以属性不会自动注入
            IAppInfoService appInfoService = (IAppInfoService)GlobalConfiguration.Configuration
                .DependencyResolver.GetService(typeof(IAppInfoService));

            var headers = actionContext.Request.Headers;
            string appKey;
            string sign;
            if (!headers.Contains("AppKey"))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content = new StringContent("AppKey为空") };
            }
            else
            {
                appKey = headers.GetValues("AppKey").Single();
            }
            if (!headers.Contains("Sign"))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content = new StringContent("Sign为空") };
            }
            else
            {
                sign = headers.GetValues("Sign").Single();
            }
            
            var appInfo = await appInfoService.GetByAppKeyAsync(appKey);
            if (appInfo == null)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content = new StringContent("AppKey错误") };
            }
            else
            {
                var queryString = actionContext.Request.GetQueryNameValuePairs().OrderBy(kv => kv.Key).ToArray();
                StringBuilder sbContent = new StringBuilder();
                for(int i=0;i<queryString.Length;i++)
                {
                    var kv = queryString[i];
                    sbContent.Append(kv.Key).Append("=").Append(kv.Value);
                    if(i< queryString.Length-1)
                    {
                        sbContent.Append("&");
                    }
                }
                string signComputed = MD5Helper.ComputeMd5(sbContent+appInfo.AppSecret);

                if (!signComputed.Equals(sign, StringComparison.OrdinalIgnoreCase))
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                                { Content = new StringContent("AppKey、AppSecret验证错误") };
                }
            }
            
            return await continuation();
        }
    }
}