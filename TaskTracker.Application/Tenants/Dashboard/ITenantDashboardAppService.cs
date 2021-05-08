using Abp.Application.Services;
using TaskTracker.Tenants.Dashboard.Dto;

namespace TaskTracker.Tenants.Dashboard
{
    public interface ITenantDashboardAppService : IApplicationService
    {
        GetMemberActivityOutput GetMemberActivity();
    }
}
