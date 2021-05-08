using System.Collections.Generic;
using TaskTracker.Authorization.Users.Dto;

namespace TaskTracker.Web.Areas.Mpa.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}