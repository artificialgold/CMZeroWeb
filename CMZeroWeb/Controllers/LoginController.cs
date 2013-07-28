using System.Web.Mvc;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Labels;

namespace CMZeroWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILabelCollectionRetriever _labelCollectionRetriever;

        public LoginController(ILabelCollectionRetriever labelCollectionRetriever)
        {
            _labelCollectionRetriever = labelCollectionRetriever;
        }
        
        [HttpGet]
        public ViewResult Index()
        {
            LoginViewModel model = new LoginViewModel
                {
                    Labels = _labelCollectionRetriever.Get("LoginPage")
                };

            return View("index", model);
        }

        //TODO: This needs doing properly, try to use 3rd party if possible
        [HttpPost]
        public RedirectToRouteResult Login(string mameInput, string passwordInput)
        {
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
