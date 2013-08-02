using System.Web.Mvc;
using CMZero.API.ServiceAgent;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Login;
using CMZero.Web.Services.ViewModelGetters;

namespace CMZeroWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardViewModelGetter _dashboardViewModelGetter;

        public DashboardController(IDashboardViewModelGetter dashboardViewModelGetter)
        {
            _dashboardViewModelGetter = dashboardViewModelGetter;
        }

        public ViewResult Index()
        {
            var model = _dashboardViewModelGetter.Get();

            return View("Index", model);
        }

    }
}
