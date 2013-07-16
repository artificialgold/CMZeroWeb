using System.Configuration;
using System.Web.Mvc;

using CMZero.API.ServiceAgent;
using CMZero.Web.Models;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Labels.Mappers;

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
            //NEED TO WIRE IN THE CONTROLLERS TO USE IOC
            const string CollectionName = "Home Page";

            LabelCollection labelCollection = _labelCollectionRetriever.Get(CollectionName);

            return View(new HomeViewModel { Labels = labelCollection });
        }
    }
}
