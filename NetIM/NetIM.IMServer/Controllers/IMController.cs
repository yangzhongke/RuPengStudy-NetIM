﻿using System;
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
        public async Task<ActionResult> Login(string phoneNum,string password)
        {
            var api = UserCenterAPIFactory.Create();
            bool isOK = await api.user.CheckLoginAsync(phoneNum, password);
            if(isOK)
            {
                var user = await api.user.GetByPhoneNumAsync(phoneNum);
               var payload = new Dictionary<string, object>();
                payload["UserId"] = user.Id;
                payload["UserNickName"] = user.NickName;
                string token = JWTHelper.Encode(payload);
                var cookieToken = new HttpCookie("Token", token) { Path = "/", Expires = DateTime.Now.AddYears(1) };
                Response.SetCookie(cookieToken);

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
            var api = UserCenterAPIFactory.Create();
            long? userId = JWTHelper.GetUserId(this.HttpContext);
            var user = await api.user.GetByIdAsync(userId.Value);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> LoadGroups()
        {
            
            long? userId = JWTHelper.GetUserId(this.HttpContext);
            var api = UserCenterAPIFactory.Create();
            var groups = await api.userGroup.GetGroupsAsync(userId.Value);
            return Json(groups);
        }

        [HttpGet]
        public ActionResult GroupChat(long groupId)
        {
            return View((object)groupId);
        }
    }
}