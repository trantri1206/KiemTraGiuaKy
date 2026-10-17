using _2380600189.Roles.Dto;
using System.Collections.Generic;

namespace _2380600189.Web.Models.Users;

public class UserListViewModel
{
    public IReadOnlyList<RoleDto> Roles { get; set; }
}
