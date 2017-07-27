using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using UserCenter.NetSDK;

namespace NetIM.IMServer
{
    public class UserCenterAPIFactory
    {
        public static UserCenterAPI Create()
        {
            var appKey = ConfigurationManager.AppSettings["UserCenter.AppKey"];
            var appSecret = ConfigurationManager.AppSettings["UserCenter.AppSecret"];
            var serverRoot = ConfigurationManager.AppSettings["UserCenter.ServerRoot"];

            UserCenterAPI api = new UserCenterAPI(appKey,appSecret,serverRoot);
            return api;
        }
    }
}