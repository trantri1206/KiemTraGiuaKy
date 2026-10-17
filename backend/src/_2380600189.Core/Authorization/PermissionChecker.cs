using Abp.Authorization;
using _2380600189.Authorization.Roles;
using _2380600189.Authorization.Users;

namespace _2380600189.Authorization;

public class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager)
    {
    }
}
