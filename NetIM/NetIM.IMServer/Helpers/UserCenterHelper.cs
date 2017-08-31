using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;

namespace NetIM.IMServer.Helpers
{
    public class UserCenterHelper
    {
        /// <summary>
        /// 得到组groupId中的用户Id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static async Task<long[]> GetGroupUserIdsAsync(long groupId)
        {
            long[] groupUserIds = null;

            //System.Runtime.Caching这个程序集中，因为不是asp.net，所以不能用asp.net的cache
            //memcache性能再高也没有Web服务器的内存性能高
            //而且组成员人员id数据量也没那么大，所以放到MemoryCache反而好
            MemoryCache cache = MemoryCache.Default;
            string GroupUserIdsKey = "GroupUserIds" + groupId;
            groupUserIds = (long[])cache.Get(GroupUserIdsKey);//从缓存中获取，提升性能
            if (groupUserIds == null)
            {
                var api = UserCenterAPIFactory.Create();
                var groupUsers = await api.userGroup.GetGroupUsersAsync(groupId);
                groupUserIds = groupUsers.Select(u => u.Id).ToArray();
                cache.Set(GroupUserIdsKey, groupUserIds, DateTimeOffset.Now.AddMinutes(1));
            }
            return groupUserIds;
        }
    }
}