using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NetIM.IMServer
{
    public class ExceptionHubPipelineModule : HubPipelineModule
    {
        protected override void OnIncomingError(ExceptionContext exceptionContext,
                                                IHubIncomingInvokerContext invokerContext)
        {
            //dynamic caller = invokerContext.Hub.Clients.Caller;
            //caller.ExceptionHandler(exceptionContext.Error.Message);
            File.AppendAllText("d:/serr.txt", exceptionContext.Error.ToString());
        }
    }
}