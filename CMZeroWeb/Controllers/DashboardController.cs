using System.Web.Mvc;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Labels;

namespace CMZeroWeb.Controllers
{
    public class DashboardController : Controller
    {
        private ILabelCollectionRetriever _labelCollectionRetriever;

        public DashboardController(ILabelCollectionRetriever labelCollectionRetriever)
        {
            _labelCollectionRetriever = labelCollectionRetriever;
        }

        public ActionResult Index()
        {
            var model = new DashboardViewModel
                                           {
                                               Labels = _labelCollectionRetriever.Get("DashboardPage")
                                           };

            return View("Index", model);
        }

    }
}
