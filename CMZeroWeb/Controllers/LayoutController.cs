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

    }
}
