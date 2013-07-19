using System.Web.Mvc;

namespace CMZeroWeb.Controllers
{
    public class LayoutController : Controller
    {
        //
        // GET: /Layout/

        public PartialViewResult NavBar()
        {
            return PartialView("_NavBar");
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
