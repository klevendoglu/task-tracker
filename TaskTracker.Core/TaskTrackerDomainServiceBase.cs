using Abp.Domain.Services;

namespace TaskTracker
{
    public abstract class TaskTrackerDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected TaskTrackerDomainServiceBase()
        {
            LocalizationSourceName = TaskTrackerConsts.LocalizationSourceName;
        }
    }
}
