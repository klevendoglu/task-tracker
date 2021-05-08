using System.Threading.Tasks;
using Abp.Application.Services;
using TaskTracker.Sessions.Dto;

namespace TaskTracker.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
