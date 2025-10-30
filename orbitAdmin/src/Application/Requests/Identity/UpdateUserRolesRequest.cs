using SchoolV01.Application.Responses.Identity;
using System.Collections.Generic;

namespace SchoolV01.Application.Requests.Identity
{
    public class UpdateUserRolesRequest
    {
        public string UserId { get; set; }
        public IList<UserRoleModel> UserRoles { get; set; }
    }
}