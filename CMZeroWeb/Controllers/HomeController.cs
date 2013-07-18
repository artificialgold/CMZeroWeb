using System.Web.Mvc;
using CMZero.Web.Models;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Labels;

namespace CMZeroWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILabelCollectionRetriever _labelCollectionRetriever;

        public HomeController(ILabelCollectionRetriever labelCollectionRetriever)
        {
            _labelCollectionRetriever = labelCollectionRetriever;
        }

        public ViewResult Index()
        {
            const string collectionName = "Home Page";

            LabelCollection labelCollection = _labelCollectionRetriever.Get(collectionName);

            return View(new HomeViewModel { Labels = labelCollection });
        }
    }
}
