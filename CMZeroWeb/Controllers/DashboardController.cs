using System.Web.Mvc;
using CMZero.Web.Models.ViewModels;

namespace CMZeroWeb.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();

            return View();
        }

    }
}
