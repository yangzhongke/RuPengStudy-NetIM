using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NetIM.IMServer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalFilters.Filters.Add(new JsonNetActionFilter());
            GlobalFilters.Filters.Add(new UserSessionFilter());

            GlobalHost.HubPipeline.AddModule(new ExceptionHubPipelineModule());
        }
    }
}
