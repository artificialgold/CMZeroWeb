using System.Web.Mvc;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Labels;

namespace CMZeroWeb.Controllers
{
    public class LayoutController : Controller
    {
        private readonly ILabelCollectionRetriever _labelCollectionRetriever;

        public LayoutController(ILabelCollectionRetriever labelCollectionRetriever)
        {
            _labelCollectionRetriever = labelCollectionRetriever;
        }

        public PartialViewResult NavBar()
        {
            BaseLabelsViewModel model = new BaseLabelsViewModel
                {
                    Labels = _labelCollectionRetriever.Get("NavBar")
                };

            return PartialView("_NavBar", model);
        }

        public PartialViewResult MastHead()
        {
            return PartialView("_MastHead");
        }

        public PartialViewResult Footer()
        {
            return PartialView("_Footer");
        }
    }
}
