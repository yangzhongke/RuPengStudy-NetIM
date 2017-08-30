using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetIM.IMServer.Helpers
{
    public class MessageHistory
    {
        public string FromUserName { get; set; }
        public long FromUserId { get; set; }

        //班级id
        public long TargetId { get; set; }
        public string Message { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}