using Abp.AutoMapper;
using TaskTracker.Authorization.Users;
using TaskTracker.Authorization.Users.Dto;
using TaskTracker.Web.Areas.Mpa.Models.Common;

namespace TaskTracker.Web.Areas.Mpa.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; private set; }

        public UserPermissionsEditViewModel(GetUserPermissionsForEditOutput output, User user)
        {
            User = user;
            output.MapTo(this);
        }
    }
}