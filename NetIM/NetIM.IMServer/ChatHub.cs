using System.Linq;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System;
using System.Runtime.Caching;
using NetIM.IMServer.Helpers;

namespace NetIM.IMServer
{
    public class ChatHub : Hub
    {
        //如果Hub中调用了异步的方法，那么一定要写成async
        //否则Hub方法只能调用一次就卡住了，后续调用就没反应了
        //不要使用WebClient的异步，都换用HttpClient
        public async Task<HubResult> Init()
        {
            HubResult hubResult = new HubResult();
            var token = this.Context.QueryString["Token"];
            var loginUserInfo = JWTHelper.GetUserInfoFromToken(token);
            if (loginUserInfo != null)
            {
                var api = UserCenterAPIFactory.Create();
                var groups = await api.userGroup.GetGroupsAsync(loginUserInfo.UserId);
                foreach (var group in groups)
                {
                    await Groups.Add(this.Context.ConnectionId, group.Id.ToString());
                }
                hubResult.Code = 0;
            }
            else
            {
                hubResult.Code = 1;
                hubResult.Message = "未登录或者已经过时";
            }
            return hubResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetId">组Id</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public async Task<HubResult> SendMessage(long targetId, string message)
        {
            HubResult hubResult = new HubResult();
            var token = this.Context.QueryString["Token"];
            var loginUserInfo = JWTHelper.GetUserInfoFromToken(token);
            if (loginUserInfo == null)
            {
                hubResult.Code = 1;
                hubResult.Message = "您未登录或者已经登录过期，请重新登录";
                return hubResult;
            }
            long[] groupUserIds = await UserCenterHelper.GetGroupUserIdsAsync(targetId);

            if (!groupUserIds.Contains(loginUserInfo.UserId))
            {
                hubResult.Code = 1;
                hubResult.Message = "您不属于组" + targetId;
                return hubResult;
            }

            Clients.Group(targetId.ToString())
                .onReceiveMessage(loginUserInfo.UserId, targetId, message);
            MessageHistoryHelper.InsertClassMsg(loginUserInfo.UserId,
                loginUserInfo.UserNickName, targetId, message);
            return hubResult;
        }
    }
}