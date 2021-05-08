using System.Web.Mvc;

namespace TaskTracker.Web.Controllers
{
    public class HomeController : TaskTrackerControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}