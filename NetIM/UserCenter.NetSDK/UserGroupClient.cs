using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCenter.NetSDK
{
    public class UserGroupClient : SDKClient
    {
        public UserGroupClient(string appKey, string appSecret, string serverRoot) 
            : base(appKey, appSecret, serverRoot)
        {
        }

        public async Task<long> GetByIdAsync(long id)
        {
            var result = await ExecutePostAsync("/UserGroup/GetById", new Dictionary<string, object> { { "id", id }});
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new ApplicationException("验证错误");
            }
            else if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("服务器返回错误" + result.StatusCode);
            }
            return JsonConvert.DeserializeObject<long>(result.Result);
        }

        public async Task<UserGroup[]> GetGroupsAsync(long userId)
        {
            var result = await ExecutePostAsync("/UserGroup/GetGroups", new Dictionary<string, object> { { "userId", userId } });
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new ApplicationException("验证错误");
            }
            else if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("服务器返回错误" + result.StatusCode);
            }
            return JsonConvert.DeserializeObject<UserGroup[]>(result.Result);
        }

        public async Task<User[]> GetGroupUsersAsync(long userGroupId)
        {
            var result = await ExecutePostAsync("/UserGroup/GetGroupUsers", new Dictionary<string, object> { { "userGroupId", userGroupId } });
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new ApplicationException("验证错误");
            }
            else if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("服务器返回错误" + result.StatusCode);
            }
            return JsonConvert.DeserializeObject<User[]>(result.Result);
        }

        public async Task AddUserToGroupAsync(long userGroupId, long userId)
        {
            var result = await ExecutePostAsync("/UserGroup/AddUserToGroup", 
                    new Dictionary<string, object> { { "userGroupId", userGroupId }, { "userId", userId } });
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new ApplicationException("验证错误");
            }
            else if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("服务器返回错误" + result.StatusCode);
            }
        }

        public async Task RemoveUserFromGroupAsync(long userGroupId, long userId)
        {
            var result = await ExecutePostAsync("/UserGroup/RemoveUserFromGroup",
                    new Dictionary<string, object> { { "userGroupId", userGroupId }, { "userId", userId } });
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new ApplicationException("验证错误");
            }
            else if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("服务器返回错误" + result.StatusCode);
            }
        }
    }
}
