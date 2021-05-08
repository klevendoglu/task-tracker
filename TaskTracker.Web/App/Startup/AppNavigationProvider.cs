using Abp.Application.Navigation;
using Abp.Localization;
using TaskTracker.Authorization;
using TaskTracker.Web.Navigation;

namespace TaskTracker.Web.App.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in angular application.
    /// See .cshtml and .js files under App/Main/views/layout/header to know how to render menu.
    /// </summary>
    public class AppNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Tenants,
                    L("Tenants"),
                    url: "host.tenants",
                    icon: "icon-globe",
                    requiredPermissionName: AppPermissions.Pages_Tenants
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Editions,
                    L("Editions"),
                    url: "host.editions",
                    icon: "icon-grid",
                    requiredPermissionName: AppPermissions.Pages_Editions
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Tenant.Dashboard,
                    L("Dashboard"),
                    url: "tenant.dashboard",
                    icon: "icon-home",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Dashboard
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Administration,
                    L("Administration"),
                    icon: "icon-wrench"
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Roles,
                        L("Roles"),
                        url: "roles",
                        icon: "icon-briefcase",
                        requiredPermissionName: AppPermissions.Pages_Administration_Roles
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Users,
                        L("Users"),
                        url: "users",
                        icon: "icon-users",
                        requiredPermissionName: AppPermissions.Pages_Administration_Users
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Host.Settings,
                        L("Settings"),
                        url: "host.settings",
                        icon: "icon-settings",
                        requiredPermissionName: AppPermissions.Pages_Administration_Host_Settings
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Tenant.Settings,
                        L("Settings"),
                        url: "tenant.settings",
                        icon: "icon-settings",
                        requiredPermissionName: AppPermissions.Pages_Administration_Tenant_Settings
                        )
                    )
                )
                // PROJECT MANAGEMENT
                .AddItem(new MenuItemDefinition(
                    PageNames.App.ProjectManagement.Manager,
                    L("Task"),
                        url: "projectManagement.manager",
                        icon: "glyphicon glyphicon-briefcase",
                        requiredPermissionName: AppPermissions.Pages_TaskTracker_Manager
                        )
                   ).AddItem(new MenuItemDefinition(
                    PageNames.App.ProjectManagement.Agent,
                        L("SubTask"),
                        url: "projectManagement.agent",
                        icon: "icon-user-following",
                        requiredPermissionName: AppPermissions.Pages_TaskTracker_Agent
                        )
                    );
            //PROJECT MANAGEMENT
            //.AddItem(new MenuItemDefinition(
            //    PageNames.App.ProjectManagement.Root,
            //    L("TaskTracker"),
            //    icon: "glyphicon glyphicon-briefcase",
            //    requiredPermissionName: AppPermissions.Pages_ProjectManagement
            //    ).AddItem(new MenuItemDefinition(
            //        PageNames.App.ProjectManagement.Manager,
            //        L("Task"),
            //        url: "projectManagement.manager",
            //        icon: "icon-settings",
            //        requiredPermissionName: AppPermissions.Pages_TaskTracker_Manager
            //        )
            //    ).AddItem(new MenuItemDefinition(
            //        PageNames.App.ProjectManagement.Agent,
            //        L("SubTask"),
            //        url: "projectManagement.agent",
            //        icon: "icon-user-following",
            //        requiredPermissionName: AppPermissions.Pages_TaskTracker_Agent
            //        )
            //    )
            //);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TaskTrackerConsts.LocalizationSourceName);
        }
    }
}
