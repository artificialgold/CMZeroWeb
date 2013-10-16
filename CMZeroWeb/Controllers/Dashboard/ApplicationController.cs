using System.Web.Mvc;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.Web.Services.ViewModelGetters;

namespace CMZeroWeb.Controllers.Dashboard
{
    public class ApplicationController : Controller
    {
        private readonly IApplicationViewModelGetter _applicationViewModelGetter;

        public ApplicationController(IApplicationViewModelGetter applicationViewModelGetter)
        {
            _applicationViewModelGetter = applicationViewModelGetter;
        }

        [HttpGet]
        public ActionResult Index(string id)
        {
            try
            {
                var model = _applicationViewModelGetter.Get(id);
                return View("~/Views/Dashboard/Application/Index.cshtml", model);
            }
            catch (ApplicationIdNotPartOfOrganisationException)
            {
                return RedirectToRoute("OhBugger");
            }
        }

        [HttpPost]
        public ActionResult Index(string applicationId, string nameInput, bool? activeCheckbox)
        {
            try
            {
                var model = _applicationViewModelGetter.Update(applicationId, nameInput, activeCheckbox.Value);
                return View("~/Views/Dashboard/Application/Index.cshtml", model);
            }
            catch (ApplicationIdNotPartOfOrganisationException)
            {
                return RedirectToRoute("OhBugger");
            }
        }

    }
}
