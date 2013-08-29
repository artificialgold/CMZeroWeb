using System;
using System.Web.Mvc;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.Web.Models.Exceptions;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Applications;
using CMZero.Web.Services.ViewModelGetters;
using CMZeroWeb.Controllers.Dashboard;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace CMZero.Web.UnitTests.Web.Controllers.Applications
{
    public class ApplicationControllerTests
    {

        public class Given_an_ApplicationController
        {
            protected ApplicationController ApplicationController;
            protected IApplicationViewModelGetter ApplicationViewModelGetter;

            [SetUp]
            public virtual void SetUp()
            {
                ApplicationViewModelGetter = Substitute.For<IApplicationViewModelGetter>();
                ApplicationController = new ApplicationController(ApplicationViewModelGetter);

            }
        }

        [TestFixture]
        public class When_I_call_index_with_an_invalid_applicationId : Given_an_ApplicationController
        {
            private RedirectToRouteResult _result;
            private const string ApplicationIdThatDoesNotExist = "xxxxxxxxxxx";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationViewModelGetter.Get(ApplicationIdThatDoesNotExist)
                                  .Returns(x => { throw new ApplicationIdNotPartOfOrganisationException(); });

                _result = (RedirectToRouteResult)ApplicationController.Index(ApplicationIdThatDoesNotExist);
            }

            [Test]
            public void it_should_redirect_to_the_error_page()
            {
                _result.RouteName.ShouldBe("OhBugger");
            }
        }

        [TestFixture]
        public class When_I_call_Index_with_a_valid_applicationId_for_the_organisation : Given_an_ApplicationController
        {
            private const string ExistingApplicationId = "IExist";
            private ViewResult _result;
            private readonly ApplicationViewModel _resultFromViewModelGetter = new ApplicationViewModel();

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationViewModelGetter.Get(ExistingApplicationId).Returns(_resultFromViewModelGetter);
                _result = (ViewResult)ApplicationController.Index(ExistingApplicationId);
            }

            [Test]
            public void it_should_return_model_from_view_model_getter()
            {
                var model = (ApplicationViewModel)_result.Model;
                model.ShouldBe(_resultFromViewModelGetter);
            }

            [Test]
            public void it_should_viewname()
            {
                _result.ViewName.ShouldBe("~/Views/Dashboard/Application/Index.cshtml");
            }

        }

        [TestFixture]
        public class When_I_update_with_applicationId_not_part_of_organisation : Given_an_ApplicationController
        {
            private RedirectToRouteResult _result;
            private const string ApplicationIdNotPartOfOrganisation = "appIdNotPartOfOrgId";
            private const string NewName = "newName";
            private const bool Active = true;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationViewModelGetter.Update(ApplicationIdNotPartOfOrganisation, NewName, Active).Returns(x => { throw new ApplicationIdNotPartOfOrganisationException(); });
                _result = (RedirectToRouteResult)ApplicationController.Index(ApplicationIdNotPartOfOrganisation, NewName, Active);
            }

            [Test]
            public void it_should_return_ohbugger_view()
            {
                _result.RouteName.ShouldBe("OhBugger");
            }
        }

        [TestFixture]
        public class When_I_update_with_applicationId_that_is_valid : Given_an_ApplicationController
        {
            private ViewResult _result;

            private readonly ApplicationViewModel _modelFromGetter = new ApplicationViewModel();

            private const string ApplicationId = "applicationId";
            private const string NewName = "newName";
            private const bool Active = true;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationViewModelGetter.Update(ApplicationId, NewName, Active).Returns(_modelFromGetter);
                _result = (ViewResult)ApplicationController.Index(ApplicationId, NewName, Active);
            }

            [Test]
            public void it_should_return_viewmodel_from_view_model_getter()
            {
                _result.Model.ShouldBe(_modelFromGetter);
            }

            [Test]
            public void it_should_return_the_view()
            {
                _result.ViewName.ShouldBe("~/Views/Dashboard/Application/Index.cshtml");
            }
        }
    }
}