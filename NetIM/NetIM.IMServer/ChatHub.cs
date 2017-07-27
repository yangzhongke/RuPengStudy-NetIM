using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace NetIM.IMServer
{
    public class ChatHub : Hub
    {
        public void SendMessage(string msgType,long targetId,string message)
        {
            //用组的id做name
            if(msgType == "private")
            {

            }
            else if(msgType == "group")
            {
                //Clients.Group(targetId.ToString()).onReceiveMessage(msgType,this.);
            }
            else
            {
                throw new ArgumentException("type参数错误");
            }
           
            //Clients.All.addNewMessageToPage(name, message);
        }
    }
}