using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserCenter.NetSDK;

namespace NetIM.IMServer.Models
{
    public class IndexModel
    {
        public string UserNickName { get; set; }
        public UserGroup[] Groups { get; set; }
    }
}