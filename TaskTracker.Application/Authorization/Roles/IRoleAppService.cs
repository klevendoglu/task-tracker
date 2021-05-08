﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TaskTracker.Authorization.Roles.Dto;

namespace TaskTracker.Authorization.Roles
{
    /// <summary>
    /// Application service that is used by 'role management' page.
    /// </summary>
    public interface IRoleAppService : IApplicationService
    {
        Task<ListResultOutput<RoleListDto>> GetRoles();

        Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdInput input);

        Task CreateOrUpdateRole(CreateOrUpdateRoleInput input);

        Task DeleteRole(EntityRequestInput input);
    }
}