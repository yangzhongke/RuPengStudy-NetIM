using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.DTO;

namespace UserCenter.IServices
{
    public interface IUserGroupService : IServiceTag
    {
        UserGroupDTO GetById(long id);
        UserGroupDTO[] GetAll();
        UserDTO[] GetGroupUsers(long userGroupId);

        void AddUserToGroup(long userGroupId, long userId);
        void RemoveUserFromGroup(long userGroupId, long userId);
    }
}
