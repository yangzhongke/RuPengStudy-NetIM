using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NetIM.IMServer.Controllers
{
    public class IMController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string phoneNum,string password)
        {
            var api = UserCenterAPIFactory.Create();
            bool isOK = await api.user.CheckLoginAsync(phoneNum, password);
            if(isOK)
            {
                var user = await api.user.GetByPhoneNumAsync(phoneNum);
                Session["UserId"] = user.Id;
                Session["UserNickName"] = user.NickName;
                return Json(new { IsOK = true, UserInfo =user});
            }
            else
            {
                return Json(new { IsOK = false});
            }            
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string userNickName = (string)Session["UserNickName"];
            return View((object)userNickName);
        }

        [HttpPost]
        public async Task<ActionResult> LoadGroups()
        {
            long userId = (long)Session["UserId"];
            var api = UserCenterAPIFactory.Create();
            var groups = await api.userGroup.GetGroupsAsync(userId);
            return Json(groups);
        }
    }
}