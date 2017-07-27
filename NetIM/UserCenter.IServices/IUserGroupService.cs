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
        Task<UserGroupDTO> GetByIdAsync(long id);
        Task<UserGroupDTO[]> GetGroupsAsync(long userId);
        Task<UserDTO[]> GetGroupUsersAsync(long userGroupId);

        Task AddUserToGroupAsync(long userGroupId, long userId);
        Task RemoveUserFromGroupAsync(long userGroupId, long userId);
    }
}
