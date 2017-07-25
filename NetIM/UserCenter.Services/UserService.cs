using RuPeng.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.DTO;
using UserCenter.IServices;
using UserCenter.Services.Models;
using System.Data.Entity;

namespace UserCenter.Services
{
    public class UserService : IUserService
    {
        public async Task<long> AddNewAsync(string phoneNum, string nickName, string password)
        {
            using (UCDbContext ctx = new UCDbContext())
            {
                if(await ctx.Users.AnyAsync(u => u.PhoneNum == phoneNum))
                {
                    throw new ApplicationException("手机号"+phoneNum+"已经存在");
                }

                User user = new User();
                user.NickName = nickName;
                user.PhoneNum = phoneNum;
                string salt = new Random().Next(10000, 99999).ToString();
                string hash = MD5Helper.ComputeMd5(password + salt);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;

                ctx.Users.Add(user);
                await ctx.SaveChangesAsync();
                return user.Id;
            }
        }

        public async Task<bool> CheckLoginAsync(string phoneNum, string password)
        {
            using (UCDbContext ctx = new UCDbContext())
            {
                var user = await ctx.Users.SingleOrDefaultAsync(e => e.PhoneNum == phoneNum);
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


        public async Task<UserDTO> GetByIdAsync(long id)
        {
            using (UCDbContext ctx = new UCDbContext())
            {
                var user = await ctx.Users.SingleOrDefaultAsync(e => e.Id == id);
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

        public async Task<UserDTO> GetByPhoneNumAsync(string phoneNum)
        {
            using (UCDbContext ctx = new UCDbContext())
            {
                var user = await ctx.Users.SingleOrDefaultAsync(e => e.PhoneNum==phoneNum);
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

        public async Task<bool> UserExistsAsync(string phoneNum)
        {
            using (UCDbContext ctx = new UCDbContext())
            {
                return await ctx.Users.AnyAsync(e=>e.PhoneNum==phoneNum);
            }
        }
    }
}
