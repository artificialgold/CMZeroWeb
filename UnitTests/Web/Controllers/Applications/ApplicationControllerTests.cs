using System;
using System.Web.Mvc;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.Web.Models.Exceptions;
using CMZero.Web.Services.Applications;
using CMZeroWeb.Controllers.Dashboard;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace CMZero.Web.UnitTests.Web.Controllers.Applications
{
    public class ApplicationControllerTests
    {

        public class Given_a_ApplicationController
        {
            protected ApplicationController ApplicationController;
            protected IApplicationService ApplicationService;

            [SetUp]
            public virtual void SetUp()
            {
                ApplicationService = NSubstitute.Substitute.For<IApplicationService>();
                ApplicationController = new ApplicationController(ApplicationService);

            }
        }

        [TestFixture]
        public class When_I_call_index_with_an_invalid_applicationId : Given_a_ApplicationController
        {
            private RedirectToRouteResult _result;
            private const string ApplicationIdThatDoesNotExist = "xxxxxxxxxxx";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationService.GetById(ApplicationIdThatDoesNotExist)
                                  .Returns(x => { throw new ApplicationIdNotPartOfOrganisationException(); });

                _result = (RedirectToRouteResult)ApplicationController.Index(ApplicationIdThatDoesNotExist);
            }

            [Test]
            public void it_should_redirect_to_the_error_page()
            {
                _result.RouteName.ShouldBe("OhBugger");
            }
        }
    }

}