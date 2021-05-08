using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using TaskTracker.Web.Areas.Mpa.Models.Common.Modals;
using TaskTracker.Web.Controllers;

namespace TaskTracker.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize]
    public class CommonController : TaskTrackerControllerBase
    {
        public PartialViewResult LookupModal(LookupModalViewModel model)
        {
            return PartialView("Modals/_LookupModal", model);
        }
    }
}