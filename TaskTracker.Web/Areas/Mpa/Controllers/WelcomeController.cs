using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using TaskTracker.Web.Controllers;

namespace TaskTracker.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize]
    public class WelcomeController : TaskTrackerControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}