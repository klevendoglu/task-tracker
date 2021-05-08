using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Castle.Core.Internal;
using TaskTracker.Widgets;
using TaskTracker.Authorization.Users.Widgets.Dto;
using Abp.Auditing;

namespace TaskTracker.Authorization.Users.Widgets
{
    [DisableAuditing]
    public class WidgetsAppService : TaskTrackerAppServiceBase, IWidgetsAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Widget> _widgetsRepository;
        private readonly IRepository<UserWidgets> _userWidgetsRepository;

        public WidgetsAppService(IRepository<User, long> userRepository, IRepository<Widget> widgetsRepository, IRepository<UserWidgets> userWidgetsRepository)
        {
            _userRepository = userRepository;
            _widgetsRepository = widgetsRepository;
            _userWidgetsRepository = userWidgetsRepository;
        }

        public async Task<ICollection<WidgetsOutput>> GetWidgets(IdInput<long> input)
        {
            var user = await _userRepository.GetAsync(input.Id);
            return (from userRoles in user.Roles
                    join widgets in _widgetsRepository.GetAll() on userRoles.RoleId equals widgets.RoleId into allWidgets
                    from aw in allWidgets
                    join userWidgtes in user.UserWidgets on aw.Id equals userWidgtes.WidgetId into widgetSet
                    from t in widgetSet.DefaultIfEmpty()
                    select new WidgetsOutput()
                    {
                        Id = aw.Id,
                        Title = aw.Title,
                        IsSelected = t != null
                    }
                ).ToList();
        }

        public async Task<UserWidgetsOutput> GetUserWidgetIds(IdInput<long> input)
        {
            var user = await _userRepository.GetAsync(input.Id);
            return new UserWidgetsOutput()
            {
                WidgetIds = user.UserWidgets.Select(x => x.WidgetId).ToList()
            };
        }

        public async Task CreateUserWidgets(CreateUserWidgetsInput input)
        {
            var user = await _userRepository.GetAsync(input.UserId);
            await _userWidgetsRepository.DeleteAsync(x => x.UserId == input.UserId);
            input.WidgetIds.ForEach(widgetId => user.UserWidgets.Add(new UserWidgets() { UserId = input.UserId, WidgetId = widgetId }));
            await  _userRepository.InsertOrUpdateAsync(user);
        }
    }
}
