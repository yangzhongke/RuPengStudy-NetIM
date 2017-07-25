using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.DTO;

namespace UserCenter.IServices
{
    public interface IUserService: IServiceTag
    {
        Task<long> AddNewAsync(string phoneNum, string nickName, string password);
        Task<bool> UserExistsAsync(string phoneNum);
        Task<bool> CheckLoginAsync(string phoneNum, string password);
        Task<UserDTO> GetByIdAsync(long id);
        Task<UserDTO> GetByPhoneNumAsync(string phoneNum);
    }
}
