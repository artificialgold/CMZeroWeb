using System.Web.Mvc;
using CMZero.API.Messages.Exceptions.ContentAreas;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.ViewModelGetters;
using CMZeroWeb.Controllers.Dashboard;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace CMZero.Web.UnitTests.Web.Controllers.Applications
{
    public class CollectionControllerTests
    {

        public class Given_a_CollectionController
        {
            protected CollectionController CollectionController;
            protected ICollectionViewModelGetter CollectionViewModelGetter;

            [SetUp]
            public virtual void SetUp()
            {
                CollectionViewModelGetter = NSubstitute.Substitute.For<ICollectionViewModelGetter>();
                CollectionController = new CollectionController();
            }
        }

        [TestFixture]
        public class When_I_call_Index_with_a_valid_collection_id : Given_a_CollectionController
        {
            private ViewResult _result;
            private const string CollectionIdThatDoesExist = "iExist";
            private readonly CollectionViewModel _collectionViewModel = new CollectionViewModel();

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                CollectionViewModelGetter.Get(CollectionIdThatDoesExist).Returns(_collectionViewModel);
                _result = (ViewResult) CollectionController.Index();
            }

            [Test]
            public void it_should_return_model_from_view_model_getter()
            {
                var model = (CollectionViewModel) _result.Model;
                model.ShouldBe(_collectionViewModel);
            }

            [Test]
            public void it_should_return_view_name()
            {
                _result.ViewName.ShouldBe("~/Views/Dashboard/Collection/Index.cshtml");
            }
        }

        [TestFixture]
        public class When_I_call_index_with_invalid_application_id : Given_a_CollectionController
        {
            private RedirectToRouteResult _result;
            private const string InvalidCollectionId = "iDoNotExist";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                CollectionViewModelGetter.Get(InvalidCollectionId).Returns(x =>
                    { throw new CollectionIdNotValidException(); });
                _result = (RedirectToRouteResult) CollectionController.Index();
            }

            [Test]
            public void it_should_redirect_to_oh_bugger_page()
            {
                _result.RouteName.ShouldBe("OhBugger");
            }
        }
    }
}