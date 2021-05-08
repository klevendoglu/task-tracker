using System;
using System.Reflection;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.Zero;
using Abp.Zero.Configuration;
using Abp.Zero.Ldap;
using Abp.Zero.Ldap.Configuration;
using TaskTracker.Authorization.Ldap;
using TaskTracker.Authorization.Roles;
using TaskTracker.Authorization.Users;
using TaskTracker.Configuration;
using TaskTracker.Debugging;
using TaskTracker.Features;
using TaskTracker.MultiTenancy;
using TaskTracker.Notifications;

namespace TaskTracker
{
    /// <summary>
    /// Core (domain) module of the application.
    /// </summary>
    [DependsOn(typeof(AbpZeroCoreModule), typeof(AbpZeroLdapModule), typeof(AbpAutoMapperModule))]
    public class TaskTrackerCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            //Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof (Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof (Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof (User);

            //Add/remove localization sources
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    "TaskTracker",
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "TaskTracker.Localization.TaskTracker"
                        )
                    )
                );

            //Adding feature providers
            Configuration.Features.Providers.Add<AppFeatureProvider>();

            //Adding setting providers
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            //Adding notification providers
            Configuration.Notifications.Providers.Add<AppNotificationProvider>();

            //Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = false;

            //Enable LDAP authentication (It can be enabled only if MultiTenancy is disabled!)
            //Configuration.Modules.ZeroLdap().Enable(typeof(AppLdapAuthenticationSource));

            //Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            if (DebugHelper.IsDebug)
            {
                //Disabling email sending in debug mode
                IocManager.Register<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
