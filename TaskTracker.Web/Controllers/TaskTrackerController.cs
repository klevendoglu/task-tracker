using Abp.Auditing;
using Abp.Web.Mvc.Authorization;
using System.Web.Mvc;

namespace TaskTracker.Web.Controllers
{
    [AbpMvcAuthorize]
    public class TaskTrackerController : TaskTrackerControllerBase
    {
        [DisableAuditing]
        public ActionResult Index()
        {
            return View("~/App/common/views/layout/layout.cshtml"); //Layout of the angular application.
        }
    }
}