using Abp.Web.Mvc.Views;

namespace TaskTracker.Web.Views
{
    public abstract class TaskTrackerWebViewPageBase : TaskTrackerWebViewPageBase<dynamic>
    {

    }

    public abstract class TaskTrackerWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected TaskTrackerWebViewPageBase()
        {
            LocalizationSourceName = TaskTrackerConsts.LocalizationSourceName;
        }
    }
}