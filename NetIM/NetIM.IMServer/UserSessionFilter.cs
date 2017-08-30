using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace NetIM.IMServer
{
    public class UserSessionFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            GenericPrincipal user = new GenericPrincipal(new GenericIdentity("666"), null);  //System.Security.Principal
            filterContext.HttpContext.User = user;
        }
    }
}