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
            long[] groupUserIds = null;

            //System.Runtime.Caching这个程序集中，因为不是asp.net，所以不能用asp.net的cache
            //memcache性能再高也没有Web服务器的内存性能高
            //而且组成员人员id数据量也没那么大，所以放到MemoryCache反而好
            MemoryCache cache = MemoryCache.Default;
            string GroupUserIdsKey = "GroupUserIds" + targetId;
            groupUserIds = (long[])cache.Get(GroupUserIdsKey);//从缓存中获取，提升性能
            if (groupUserIds == null)
            {
                var api = UserCenterAPIFactory.Create();
                var groupUsers = await api.userGroup.GetGroupUsersAsync(targetId);
                groupUserIds = groupUsers.Select(u => u.Id).ToArray();
                cache.Set(GroupUserIdsKey, groupUserIds, DateTimeOffset.Now.AddMinutes(1));
            }

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