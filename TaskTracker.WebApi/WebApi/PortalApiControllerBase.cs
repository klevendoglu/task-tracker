using Abp.WebApi.Controllers;

namespace TaskTracker.WebApi
{
    public abstract class TaskTrackerApiControllerBase : AbpApiController
    {
        protected TaskTrackerApiControllerBase()
        {
            LocalizationSourceName = TaskTrackerConsts.LocalizationSourceName;
        }
    }
}