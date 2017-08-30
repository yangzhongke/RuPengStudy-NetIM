using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetIM.IMServer.Helpers
{
    public class MessageHistoryHelper
    {
        private static readonly string RedisPrefix = "RuPengIM.MessageHistoryList.";
        /// <summary>
        /// 插入班级群聊消息
        /// </summary>
        /// <param name="fromUserId"></param>
        /// <param name="toUserId"></param>
        /// <param name="msg"></param>
        public static void InsertClassMsg(long fromUserId,string fromUserName, long classId, string msg)
        {
            var redis = RedisHelper.Connection.GetDatabase();
            MessageHistory record = new MessageHistory();
            record.CreateDateTime = DateTime.Now;
            record.FromUserId = fromUserId;
            record.FromUserName = fromUserName;
            record.Message = msg;
            record.TargetId = classId;
            string recordJson = JsonConvert.SerializeObject(record);
            redis.ListLeftPush(RedisPrefix + "Class." + classId, recordJson);
        }

        /// <summary>
        /// 获取班级ClassId的历史消息
        /// </summary>
        public static IEnumerable<MessageHistory> GetClassHistoryMsgs(long classId, int count)
        {
            List<MessageHistory> result = new List<MessageHistory>();
            IEnumerable<RedisValue> data;

            var redis = RedisHelper.Connection.GetDatabase();
            data = redis.ListRange(RedisPrefix + "Class." + classId, 0, count);
            foreach (string item in data)
            {
                result.Add(JsonConvert.DeserializeObject<MessageHistory>(item));
            }
            result.Sort((e1, e2) => e1.CreateDateTime.CompareTo(e2.CreateDateTime));
            return result;
        }
    }
}