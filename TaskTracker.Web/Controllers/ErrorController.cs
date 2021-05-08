using System.Web.Mvc;
using Abp.Auditing;

namespace TaskTracker.Web.Controllers
{
    public class ErrorController : TaskTrackerControllerBase
    {
        [DisableAuditing]
        public ActionResult E404()
        {
            return View();
        }
    }
}