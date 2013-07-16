using System.Web.Mvc;

using CMZero.Web.Models;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Labels;

using CMZeroWeb.Controllers;

using NSubstitute;

using NUnit.Framework;

using Shouldly;

namespace CMZero.Web.UnitTests.Web.Controllers
{
    public class HomeControllerTests
    {
        public class Given_a_HomeController
        {
            protected HomeController _homeController;

            protected ILabelCollectionRetriever _labelCollectionRetriever;

            [SetUp]
            public virtual void SetUp()
            {
                _labelCollectionRetriever = Substitute.For<ILabelCollectionRetriever>();
                _homeController = new HomeController(_labelCollectionRetriever);
            }
        }

        public class When_I_call_Index : Given_a_HomeController
        {
            private ViewResult _result;

            private readonly LabelCollection _labelsFromRetriever = new LabelCollection();

            private const string CollectionName = "Home Page";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                _labelCollectionRetriever.Get(CollectionName).Returns(_labelsFromRetriever);
                _result = _homeController.Index();
            }

            [Test]
            public void it_should_return_model_with_LabelCollection_from_retriever()
            {
                var model = (HomeViewModel)_result.Model;
                model.Labels.ShouldBe(_labelsFromRetriever);
            }
        }
    }
}