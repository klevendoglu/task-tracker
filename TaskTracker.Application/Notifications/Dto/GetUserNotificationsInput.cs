using Abp.Notifications;
using TaskTracker.Dto;

namespace TaskTracker.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public string Filter { get; set; }

        public UserNotificationState? State { get; set; }
    }
}