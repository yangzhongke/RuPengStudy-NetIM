using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UserCenter.DTO;
using UserCenter.IServices;

namespace UserCenter.OpenAPI.Controllers
{
    public class UserController : ApiController
    {
        public IUserService UserService { get; set; }


        [HttpGet]
        public async Task<long> AddNew(string phoneNum, string nickName, string password)
        {
            return await UserService.AddNewAsync(phoneNum, nickName, password);
        }

        [HttpGet]
        public async Task<bool> UserExists(string phoneNum)
        {
            return await UserService.UserExistsAsync(phoneNum);
        }

        [HttpGet]
        public async Task<bool> CheckLogin(string phoneNum, string password)
        {
            return await UserService.CheckLoginAsync(phoneNum, password);
        }

        //关于各大平台API为什么不使用restful的风格 http://bbs.csdn.net/topics/390944890
        //使用PostMan调试webapi  https://www.getpostman.com/
        [HttpGet]
        public async Task<UserDTO> GetById(long id)
        {
            return await UserService.GetByIdAsync(id);
        }

        [HttpGet]
        public async Task<UserDTO> GetByPhoneNum(string phoneNum)
        {
            return await UserService.GetByPhoneNumAsync(phoneNum);
        }
    }
}
