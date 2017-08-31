using NetIM.IMServer.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public async Task<ActionResult> Login(string phoneNum, string password)
        {
            var api = UserCenterAPIFactory.Create();
            bool isOK = await api.user.CheckLoginAsync(phoneNum, password);
            if (isOK)
            {
                var user = await api.user.GetByPhoneNumAsync(phoneNum);
                var payload = new Dictionary<string, object>();
                payload["UserId"] = user.Id;
                payload["UserNickName"] = user.NickName;
                string token = JWTHelper.Encode(payload);
                var cookieToken = new HttpCookie("Token", token) { Path = "/", Expires = DateTime.Now.AddYears(1) };
                Response.SetCookie(cookieToken);

                return Json(new { IsOK = true, UserInfo = user });
            }
            else
            {

                return Json(new { IsOK = false });
            }
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var api = UserCenterAPIFactory.Create();
            var userInfo = JWTHelper.GetUserInfo(this.HttpContext);
            var user = await api.user.GetByIdAsync(userInfo.UserId);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> LoadGroups()
        {
            var userInfo = JWTHelper.GetUserInfo(this.HttpContext);
            if (userInfo == null)
            {
                return Content("未登录");
            }
            var api = UserCenterAPIFactory.Create();
            var groups = await api.userGroup.GetGroupsAsync(userInfo.UserId);
            return Json(groups);
        }

        [HttpGet]
        public async Task<ActionResult> GroupChat(long groupId)
        {
            var userInfo = JWTHelper.GetUserInfo(this.HttpContext);
            if (userInfo == null)
            {
                return Content("未登录");
            }
            long[] groupUserIds = await UserCenterHelper.GetGroupUserIdsAsync(groupId);
            if(!groupUserIds.Contains(userInfo.UserId))
            {
                return Content("您不属于这个组");
            }

            ViewBag.groupId = groupId;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoadHistoryMessages(long groupId)
        {
            var userInfo = JWTHelper.GetUserInfo(this.HttpContext);
            if (userInfo == null)
            {
                return Json(new { status = 1,msg = "未登录" }); 
            }
            long[] groupUserIds = await UserCenterHelper.GetGroupUserIdsAsync(groupId);
            if (!groupUserIds.Contains(userInfo.UserId))
            {
                return Json(new { status = 2, msg = "您不属于这个组" });
            }

            var msgs = MessageHistoryHelper.GetClassHistoryMsgs(groupId, 50);
            return Json(new { status=0,data=msgs});
        }
    }
}