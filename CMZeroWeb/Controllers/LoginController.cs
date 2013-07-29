using System.Web.Mvc;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Login;

namespace CMZeroWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILabelCollectionRetriever _labelCollectionRetriever;

        private readonly IFormsAuthenticationService _formsAuthenticationService;

        public LoginController(ILabelCollectionRetriever labelCollectionRetriever, IFormsAuthenticationService formsAuthenticationService)
        {
            _labelCollectionRetriever = labelCollectionRetriever;
            _formsAuthenticationService = formsAuthenticationService;
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
        public RedirectToRouteResult Login(string nameInput, string passwordInput)
        {
            var x = (string)RouteData.Values["nameInput"];
            _formsAuthenticationService.SignIn(nameInput, "test", "test", true);

            return RedirectToAction("Index", "Dashboard");
        }


    }
}
