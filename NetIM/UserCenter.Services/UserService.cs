using RuPeng.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.DTO;
using UserCenter.IServices;
using UserCenter.Services.Models;

namespace UserCenter.Services
{
    public class UserService : IUserService
    {
        public long AddNew(string phoneNum, string nickName, string password)
        {
            using (UCDbContext ctx = new UCDbContext())
            {
                User user = new User();
                user.NickName = nickName;
                user.PhoneNum = phoneNum;
                string salt = new Random().Next(10000, 99999).ToString();
                string hash = MD5Helper.ComputeMd5(password + salt);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;

                ctx.Users.Add(user);
                ctx.SaveChanges();
                return user.Id;
            }
        }

        public bool CheckLogin(string phoneNum, string password)
        {
            using (UCDbContext ctx = new UCDbContext())
            {
                var user = ctx.Users.SingleOrDefault(e => e.PhoneNum == phoneNum);
                if(user==null)
                {
                    return false;
                }
                string inputHash = MD5Helper.ComputeMd5(password + user.PasswordSalt);
                return user.PasswordHash == inputHash;
            }
        }

        private static UserDTO ToDTO(User user)
        {
            UserDTO dto = new UserDTO();
            dto.Id = user.Id;
            dto.NickName = user.NickName;
            dto.PhoneNum = user.PhoneNum;
            return dto;
        }


        public UserDTO GetById(long id)
        {
            using (UCDbContext ctx = new UCDbContext())
            {
                var user = ctx.Users.SingleOrDefault(e => e.Id == id);
                if(user==null)
                {
                    return null;
                }
                else
                {
                    return ToDTO(user);
                }
            }
        }

        public UserDTO GetByPhoneNum(string phoneNum)
        {
            using (UCDbContext ctx = new UCDbContext())
            {
                var user = ctx.Users.SingleOrDefault(e => e.PhoneNum==phoneNum);
                if (user == null)
                {
                    return null;
                }
                else
                {
                    return ToDTO(user);
                }
            }
        }

        public bool UserExists(string phoneNum)
        {
            using (UCDbContext ctx = new UCDbContext())
            {
                return ctx.Users.Any(e=>e.PhoneNum==phoneNum);
            }
        }
    }
}
