using System.Collections.Generic;
using CMZero.API.Messages;
using CMZero.API.ServiceAgent;
using CMZero.Web.Models.Exceptions;
using CMZero.Web.Services.Applications;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace CMZero.Web.UnitTests.Services.Applications
{
    public class ApplicationServiceTests
    {
        public class Given_an_ApplicationService
        {
            protected ApplicationService ApplicationService;
            protected IApplicationsServiceAgent ApplicationServiceAgent;

            [SetUp]
            public void SetUp()
            {
                ApplicationServiceAgent = Substitute.For<IApplicationsServiceAgent>();
                ApplicationService = new ApplicationService(ApplicationServiceAgent);
            }
        }

        [TestFixture]
        public class When_I_call_GetByOrganisationId_with_invalid_organisationId
            : Given_an_ApplicationService
        {
            private OrganisationIdNotValidException _exception;
            private const string OrganisationIdThatDoesNotExist = "orgId";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationService.GetByOrganisationId(OrganisationIdThatDoesNotExist)
                                  .Returns(
                                      x =>
                                          {
                                              throw new API.Messages.Exceptions.Organisations.
                                                  OrganisationIdNotValidException();
                                          });

                try
                {
                    ApplicationService.GetByOrganisationId(OrganisationIdThatDoesNotExist);
                }
                catch (OrganisationIdNotValidException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_throw_OrganisationIdNotValidException()
            {
                _exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_GetByOrganisationId_with_valid_id : Given_an_ApplicationService
        {
            private IEnumerable<Application> _result;
            private readonly List<Application> _returnThis = new List<Application>();
            private const string OrganisationId = "orgId";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationServiceAgent.GetByOrganisation(OrganisationId).Returns(_returnThis);
                _result = ApplicationService.GetByOrganisationId(OrganisationId);
            }

            [Test]
            public void it_should_return_the_result_from_service_agent()
            {
                _result.ShouldBe(_returnThis);
            }
        }
    }
}