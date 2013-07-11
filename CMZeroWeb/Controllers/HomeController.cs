using System.Configuration;
using System.Web.Mvc;

using CMZero.API.ServiceAgent;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Labels.Mappers;

namespace CMZeroWeb.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            const string CollectionId = "821242ab-86ea-4253-b6fd-7d7caf5ddc70";

            var labelCollectionRetriever = new LabelCollectionRetriever(new LabelCollectionMapper(new ContentAreaMapper()), new ContentAreasServiceAgent(ConfigurationManager.AppSettings["BaseUri"]));

            var labelCollection = labelCollectionRetriever.Get(ConfigurationManager.AppSettings[""]);

            return View(new HomeViewModel
            {
                Labels = labelCollection
            });
        }
    }
}
