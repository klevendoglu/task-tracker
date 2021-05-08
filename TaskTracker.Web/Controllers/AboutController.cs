using System.Web.Mvc;

namespace TaskTracker.Web.Controllers
{
    public class AboutController : TaskTrackerControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}