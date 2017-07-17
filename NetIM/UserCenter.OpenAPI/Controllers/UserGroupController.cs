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
    public class UserGroupController : ApiController
    {
        public IUserGroupService GroupService { get; set; }

        public UserGroupDTO GetById(long id)
        {
            return GroupService.GetById(id);
        }
        public UserGroupDTO[] GetAll()
        {
            return GroupService.GetAll();
        }

        public UserDTO[] GetGroupUsers(long userGroupId)
        {
            return GroupService.GetGroupUsers(userGroupId);
        }

        public void AddUserToGroup(long userGroupId, long userId)
        {
            GroupService.AddUserToGroup(userGroupId, userId);
        }
        public void RemoveUserFromGroup(long userGroupId, long userId)
        {
            GroupService.RemoveUserFromGroup(userGroupId, userId);
        }
    }
}
