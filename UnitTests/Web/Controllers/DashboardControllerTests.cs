using System.Web.Mvc;
using CMZero.Web.Models;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.ViewModelGetters;
using CMZeroWeb.Controllers;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace CMZero.Web.UnitTests.Web.Controllers
{
    public class DashboardControllerTests
    {

        public class Given_a_DashboardController
        {
            protected DashboardController DashboardController;
            protected IDashboardViewModelGetter DashboardViewModelGetter;
            protected ILabelCollectionRetriever LabelCollectionRetriever;

            [SetUp]
            public virtual void SetUp()
            {
                DashboardViewModelGetter = Substitute.For<IDashboardViewModelGetter>();
                LabelCollectionRetriever = Substitute.For<ILabelCollectionRetriever>();
                DashboardController = new DashboardController(DashboardViewModelGetter, LabelCollectionRetriever);
            }
        }

        [TestFixture]
        public class When_I_call_Index : Given_a_DashboardController
        {
            private ViewResult _result;
            private readonly DashboardViewModel _modelFromGetter = new DashboardViewModel();

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                DashboardViewModelGetter.Get().Returns(_modelFromGetter);
                _result = DashboardController.Index();
            }

            [Test]
            public void it_should_return_model_from_view_model_getter()
            {
                _result.Model.ShouldBe(_modelFromGetter);
            }

            [Test]
            public void it_should_return_index_view()
            {
                _result.ViewName.ShouldBe("Index");
            }
        }

        [TestFixture]
        public class When_I_call_Error : Given_a_DashboardController
        {
            private ViewResult _result;
            private readonly LabelCollection _labelsFromRetriever = new LabelCollection();

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                LabelCollectionRetriever.Get("DashboardErrorPage");
                _result = DashboardController.Error();
            }

            [Test]
            public void it_should_return_error_view()
            {
                _result.ViewName.ShouldBe("Error");
            }

            [Test]
            public void it_should_return_labels_from_retriever_in_model()
            {
                var model = (DashboardErrorViewModel)_result.Model;
                model.Labels.ShouldBe(_labelsFromRetriever);
            }
        }
    }
}