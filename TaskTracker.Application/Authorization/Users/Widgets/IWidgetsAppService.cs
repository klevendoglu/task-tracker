using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TaskTracker.Authorization.Users.Widgets.Dto;

namespace TaskTracker.Authorization.Users.Widgets
{
    public interface IWidgetsAppService : IApplicationService
    {
        Task<ICollection<WidgetsOutput>> GetWidgets(IdInput<long> input);
        Task<UserWidgetsOutput> GetUserWidgetIds(IdInput<long> input);
        Task CreateUserWidgets(CreateUserWidgetsInput input);
    }
}
