using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserCenter.NetSDK.Tests
{
    [TestClass]
    public class UnitTestUser
    {
        private string appKey = "fasdf2236afasdZ98";
        private string appSecret = "fsadfa$900jiuy7832yhuXz";
        private string serverRoot = "http://localhost:49420/api";
        

        [TestMethod]
        public void TestMethodCheckLoginAsync()
        {
            UserCenterAPI api = new UserCenterAPI(appKey, appSecret, serverRoot);
            Assert.IsTrue(api.user.CheckLoginAsync("18918918189", "123").Result);
            Assert.IsFalse(api.user.CheckLoginAsync("3333", "123").Result);
            Assert.IsFalse(api.user.CheckLoginAsync("18918918189", "333").Result);
        }
    }
}
