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
            string authValue = Encoding.Default.GetString(Convert.FromBase64String(headers.Authorization.Parameter));
            string[] authValues = authValue.Split(':');
            string appKey = authValues[0];
            string appSecret = authValues[1];
            if (string.IsNullOrEmpty(appKey))
            {
                return  new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content=new StringContent("AppKey为空")};
            }
            if (string.IsNullOrEmpty(appSecret))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content = new StringContent("appSecret为空") };
            }

            var appInfo = await appInfoService.GetByAppKeyAsync(appKey);
            if (appInfo == null)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content = new StringContent("AppKey错误") };
            }
            else
            {
                if (!appSecret.Equals(appInfo.AppSecret, StringComparison.OrdinalIgnoreCase))
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                                { Content = new StringContent("AppKey、AppSecret验证错误") };
                }
            }
            
            return await continuation();
        }
    }
}