using System;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace NetIM.IMServer
{
    public class ChatHub : Hub
    {
        //如果Hub中调用了异步的方法，那么一定要写成async
        //否则Hub方法只能调用一次就卡住了，后续调用就没反应了
        //不要使用WebClient的异步，都换用HttpClient
        public async Task Init()
        {            
            var token = this.Context.QueryString["Token"];
            long? userId = JWTHelper.GetUserIdFromToken(token);
            if(userId!=null)
            {
                var api = UserCenterAPIFactory.Create();
                var groups = await api.userGroup.GetGroupsAsync(userId.Value);
                foreach (var group in groups)
                {
                    await Groups.Add(this.Context.ConnectionId, group.Id.ToString());
                }
            }          
        }

        public async Task SendMessage(string msgType,long targetId,string message)
        {
            var token = this.Context.QueryString["Token"];
            long? userId = JWTHelper.GetUserIdFromToken(token);
            //用组的id做name
            if (msgType == "private")
            {

            }
            else if(msgType == "group")
            {
                Clients.Group(targetId.ToString()).onReceiveMessage(msgType, userId, targetId, message);
            }
            else
            {
                throw new ArgumentException("type参数错误");
            }
           
            //Clients.All.addNewMessageToPage(name, message);
        }
    }
}