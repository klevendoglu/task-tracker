using System.Reflection;
using Abp.Modules;
using TaskTracker.Authorization;

namespace TaskTracker
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(typeof(TaskTrackerCoreModule))]
    public class TaskTrackerApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //Custom DTO auto-mappings
            CustomDtoMapper.CreateMappings();
        }
    }
}
