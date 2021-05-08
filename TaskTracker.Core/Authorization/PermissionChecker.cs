using Abp.Authorization;
using TaskTracker.Authorization.Roles;
using TaskTracker.Authorization.Users;
using TaskTracker.MultiTenancy;

namespace TaskTracker.Authorization
{
    /// <summary>
    /// Implements <see cref="PermissionChecker"/>.
    /// </summary>
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
