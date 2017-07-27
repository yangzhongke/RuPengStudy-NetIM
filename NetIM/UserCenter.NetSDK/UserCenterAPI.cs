using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCenter.NetSDK
{
    public class UserCenterAPI
    {

        public UserCenterAPI(String appKey, String appSecret, string serverRoot= "http://localhost:49420/api")
        {

            user = new UserClient(appKey, appSecret, serverRoot);
            userGroup = new UserGroupClient(appKey, appSecret, serverRoot);
        }

        public UserClient user { get; private set; }
        public UserGroupClient userGroup { get; private set; }
    }
}
