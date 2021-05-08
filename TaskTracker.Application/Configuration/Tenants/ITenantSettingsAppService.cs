using System.Threading.Tasks;
using Abp.Application.Services;
using TaskTracker.Configuration.Tenants.Dto;

namespace TaskTracker.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);
    }
}
