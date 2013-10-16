using System.Web.Mvc;
using CMZero.API.Messages.Exceptions.Organisations;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.ViewModelGetters;

namespace CMZeroWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardViewModelGetter _dashboardViewModelGetter;
        private readonly ILabelCollectionRetriever _labelCollectionRetriever;

        public DashboardController(IDashboardViewModelGetter dashboardViewModelGetter, ILabelCollectionRetriever labelCollectionRetriever)
        {
            _dashboardViewModelGetter = dashboardViewModelGetter;
            _labelCollectionRetriever = labelCollectionRetriever;
        }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var model = _dashboardViewModelGetter.Get();

                return View("Index", model);
            }
            catch (OrganisationIdNotValidException)
            {
                return RedirectToRoute("OhBugger");
            }
        }

        public ViewResult Error()
        {
            var model = new DashboardErrorViewModel
            {
                Labels = _labelCollectionRetriever.Get("DashboardErrorPage")
            };

            return View("Error", model);
        }

    }
}
