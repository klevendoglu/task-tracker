using System.Threading.Tasks;
using Abp.Application.Services;
using TaskTracker.Configuration.Host.Dto;

namespace TaskTracker.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
