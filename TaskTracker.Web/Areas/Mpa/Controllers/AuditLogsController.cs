using System.Web.Mvc;
using Abp.Auditing;
using Abp.Web.Mvc.Authorization;
using TaskTracker.Authorization;
using TaskTracker.Web.Controllers;

namespace TaskTracker.Web.Areas.Mpa.Controllers
{
    [DisableAuditing]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_AuditLogs)]
    public class AuditLogsController : TaskTrackerControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}