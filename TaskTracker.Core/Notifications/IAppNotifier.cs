using System.Threading.Tasks;
using Abp;
using Abp.Notifications;
using TaskTracker.Authorization.Users;
using TaskTracker.MultiTenancy;

namespace TaskTracker.Notifications
{
    public interface IAppNotifier
    {
        Task WelcomeToTheApplicationAsync(User user);

        Task NewUserRegisteredAsync(User user);

        Task NewTenantRegisteredAsync(Tenant tenant);

        Task NewTaskAssignedEventAsync(string message, string filterText, UserIdentifier targetUserId);

        Task NewTaskClosedEventAsync(string message, string filterText, UserIdentifier targetUserId);

        Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info);
    }
}
