using System.Linq;
using Abp;
using Abp.Authorization;
using TaskTracker.Authorization;
using TaskTracker.Tenants.Dashboard.Dto;
using Abp.Auditing;

namespace TaskTracker.Tenants.Dashboard
{
    [AbpAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    [DisableAuditing]
    public class TenantDashboardAppService : TaskTrackerAppServiceBase, ITenantDashboardAppService
    {
        public GetMemberActivityOutput GetMemberActivity()
        {
            //Generating some random data. We could get numbers from database...
            return new GetMemberActivityOutput
                   {
                       TotalMembers = Enumerable.Range(0, 13).Select(i => RandomHelper.GetRandom(15, 40)).ToList(),
                       NewMembers = Enumerable.Range(0, 13).Select(i => RandomHelper.GetRandom(3, 15)).ToList()
                   };
        }
    }
}