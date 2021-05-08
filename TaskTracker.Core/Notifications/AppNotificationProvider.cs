using Abp.Authorization;
using Abp.Localization;
using Abp.Notifications;
using TaskTracker.Authorization;

namespace TaskTracker.Notifications
{
    public class AppNotificationProvider : NotificationProvider
    {
        public override void SetNotifications(INotificationDefinitionContext context)
        {
            context.Manager.Add(
                new NotificationDefinition(
                    AppNotificationNames.NewUserRegistered,
                    displayName: L("NewUserRegisteredNotificationDefinition"),
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Users)
                    )
                );

            context.Manager.Add(
                new NotificationDefinition(
                    AppNotificationNames.NewTenantRegistered,
                    displayName: L("NewTenantRegisteredNotificationDefinition"),
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants)
                    )
                );

            context.Manager.Add(
                new NotificationDefinition(
                    AppNotificationNames.NewTaskAssigned,
                    displayName: L("NewTaskAssignedNotificationDefinition"),
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard)
                    )
                );

            context.Manager.Add(
                new NotificationDefinition(
                    AppNotificationNames.NewTaskClosed,
                    displayName: L("NewTaskClosedNotificationDefinition"),
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard)
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TaskTrackerConsts.LocalizationSourceName);
        }
    }
}
