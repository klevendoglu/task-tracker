using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TaskTracker.Authorization.Users.Dto;

namespace TaskTracker.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<ListResultOutput<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}
