using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TaskTracker.Notifications.Dto;

namespace TaskTracker.Notifications
{
    public interface INotificationAppService : IApplicationService
    {
        Task<GetNotificationsOutput> GetUserNotifications(GetUserNotificationsInput input);

        Task SetAllNotificationsAsRead();

        Task SetNotificationAsRead(IdInput<Guid> input);

        Task<GetNotificationSettingsOutput> GetNotificationSettings();
        
        Task UpdateNotificationSettings(UpdateNotificationSettingsInput input);
    }
}