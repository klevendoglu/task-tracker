using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TaskTracker.Authorization.Dto;

namespace TaskTracker.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput : IOutputDto
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}