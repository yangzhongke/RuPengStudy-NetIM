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
        long AddNew(string phoneNum, string nickName, string password);
        bool UserExists(string phoneNum);
        bool CheckLogin(string phoneNum, string password);
        UserDTO GetById(long id);
        UserDTO GetByPhoneNum(string phoneNum);
    }
}
