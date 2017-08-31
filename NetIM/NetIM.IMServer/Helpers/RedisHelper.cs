using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NetIM.IMServer.Helpers
{
    public class RedisHelper
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string[] redisServers = ConfigurationManager.AppSettings["RedisServers"].Split(',',';');
            ConfigurationOptions option = new ConfigurationOptions();
            option.ConnectTimeout = 30 * 1000;
            option.ResponseTimeout = 30 * 1000;
            option.SyncTimeout = 30 * 1000;
            foreach(var server in redisServers)
            {
                option.EndPoints.Add(server);
            }
            return ConnectionMultiplexer.Connect(option);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}