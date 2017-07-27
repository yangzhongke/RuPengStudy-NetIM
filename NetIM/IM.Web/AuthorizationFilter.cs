using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace IM.Web
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public bool AllowMultiple => throw new NotImplementedException();

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            string token;
            var header = actionContext.Request.Headers;
            if(!header.Contains("Token"))
            {
                //如果没有Token
                //如果Action标注了AllowAnonymousAttribute则可以匿名访问
                if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
                {
                    return await continuation();
                }
                else
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized) { Content = new StringContent("Token为空") };
                }
            }
            else
            {
                token = header.GetValues("Token").Single();
                return null;
            }
        }
    }
}