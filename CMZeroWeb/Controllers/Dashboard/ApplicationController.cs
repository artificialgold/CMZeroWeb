using System.Web.Mvc;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.Web.Services.Applications;

namespace CMZeroWeb.Controllers.Dashboard
{
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public ActionResult Index(string id)
        {
            try
            {
                _applicationService.GetById(id);
            }
            catch (ApplicationIdNotPartOfOrganisationException)
            {
                return RedirectToRoute("OhBugger");
            }

            return View("~/Views/Dashboard/Application/Index.cshtml");
        }
    }
}
