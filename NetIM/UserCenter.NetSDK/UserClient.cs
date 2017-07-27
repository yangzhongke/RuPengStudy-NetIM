using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCenter.NetSDK
{
    public class UserClient : SDKClient
    {
        public UserClient(string appKey, string appSecret, string serverRoot) : base(appKey, appSecret, serverRoot)
        {
        }

        public async Task<long> AddNewAsync(string phoneNum, string nickName, string password)
        {
            var result = await ExecutePostAsync("/User/AddNew", new Dictionary<string, object> { { "phoneNum", phoneNum },
                    { "nickName", nickName }, { "password", password }});
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new ApplicationException("验证错误");
            }
            else if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("服务器返回错误" + result.StatusCode);
            }
            return JsonConvert.DeserializeObject<long>(result.Result);//  Install-Package newtonsoft.json
        }
        public async Task<bool> UserExistsAsync(string phoneNum)
        {
            var result = await ExecutePostAsync("/User/UserExists", new Dictionary<string, object> { { "phoneNum", phoneNum } });
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new ApplicationException("验证错误");
            }
            else if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("服务器返回错误" + result.StatusCode);
            }
            return JsonConvert.DeserializeObject<bool>(result.Result);//  Install-Package newtonsoft.json
        }

        public async Task<bool> CheckLoginAsync(string phoneNum, string password)
        {
            var result = await ExecutePostAsync("/User/CheckLogin",
                new Dictionary<string, object> { { "phoneNum", phoneNum }, { "password", password } });
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new ApplicationException("验证错误");
            }
            else if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("服务器返回错误" + result.StatusCode);
            }
            return JsonConvert.DeserializeObject<bool>(result.Result);
        }

        public async Task<User> GetByIdAsync(long id)
        {
            var result = await ExecutePostAsync("/User/GetById",
                new Dictionary<string, object> { { "id", id } });
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new ApplicationException("验证错误");
            }
            else if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("服务器返回错误" + result.StatusCode);
            }
            return JsonConvert.DeserializeObject<User>(result.Result);
        }

        public async Task<User> GetByPhoneNumAsync(string phoneNum)
        {
            var result = await ExecutePostAsync("/User/GetByPhoneNum",
                 new Dictionary<string, object> { { "phoneNum", phoneNum } });
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new ApplicationException("验证错误");
            }
            else if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("服务器返回错误" + result.StatusCode);
            }
            return JsonConvert.DeserializeObject<User>(result.Result);
        }
    }
}
