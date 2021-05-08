using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using TaskTracker.Authorization;
using TaskTracker.Web.Controllers;

namespace TaskTracker.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class DashboardController : TaskTrackerControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}