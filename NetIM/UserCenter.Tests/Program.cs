using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.NetSDK;

namespace UserCenter.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            UserCenterAPI api = new UserCenterAPI("fasdf2236afasdZ98", "fsadfa$900jiuy7832yhuXz", 
                "http://localhost:8888/api");
            var b = api.user.AddNewAsync("189", "189", "123").Result;
            Console.WriteLine(b);
            Console.ReadKey();
        }
    }
}
