using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using TaskTracker.EntityFramework;

namespace TaskTracker.Migrator
{
    [DependsOn(typeof(TaskTrackerDataModule))]
    public class TaskTrackerMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<TaskTrackerDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}