using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserCenter.OpenAPI
{
    public class AuthenticationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var headers = filterContext.RequestContext.HttpContext.Request.Headers;
           // string appKey = headers["AppKey"]
        }
    }
}