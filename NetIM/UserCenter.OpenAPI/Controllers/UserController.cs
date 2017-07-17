using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserCenter.DTO;
using UserCenter.IServices;

namespace UserCenter.OpenAPI.Controllers
{
    public class UserController : ApiController
    {
        public IUserService UserService { get; set; }

        public long AddNew(string phoneNum, string nickName, string password)
        {
            return UserService.AddNew(phoneNum, nickName, password);
        }

        public bool UserExists(string phoneNum)
        {
            return UserService.UserExists(phoneNum);
        }

        public bool CheckLogin(string phoneNum, string password)
        {
            return UserService.CheckLogin(phoneNum, password);
        }

        //关于各大平台API为什么不使用restful的风格 http://bbs.csdn.net/topics/390944890
        //使用PostMan调试webapi  https://www.getpostman.com/
        [Route("user/GetById/{id}")]
        [HttpGet]
        public UserDTO GetById(long id)
        {
            return UserService.GetById(id);
        }

        [Route("user/GetByPhoneNum/{phoneNum}")]
        [HttpGet]
        public UserDTO GetByPhoneNum(string phoneNum)
        {
            return UserService.GetByPhoneNum(phoneNum);
        }
    }
}
