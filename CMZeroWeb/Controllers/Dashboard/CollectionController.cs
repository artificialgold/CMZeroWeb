using System.Web.Mvc;
using CMZero.API.Messages.Exceptions.ContentAreas;
using CMZero.Web.Services.ViewModelGetters;

namespace CMZeroWeb.Controllers.Dashboard
{
    public class CollectionController : Controller
    {
        private readonly ICollectionViewModelGetter _collectionViewModelGetter;

        public CollectionController(ICollectionViewModelGetter collectionViewModelGetter)
        {
            _collectionViewModelGetter = collectionViewModelGetter;
        }

        [HttpGet]
        public ActionResult Index(string id)
        {
            try
            {
                var model = _collectionViewModelGetter.Get(id);
            }
            catch (CollectionIdNotValidException)
            {
                return RedirectToRoute("OhBugger");
            }


            return View();
        }

    }
}
