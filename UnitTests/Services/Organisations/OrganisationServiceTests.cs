using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Organisations;
using CMZero.API.ServiceAgent;
using CMZero.Web.Services.Login;
using CMZero.Web.Services.Organisations;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace CMZero.Web.UnitTests.Services.Organisations
{
    public class OrganisationServiceTests
    {
        public class Given_an_OrganisationService
        {
            protected OrganisationService OrganisationService;
            protected IFormsAuthenticationService FormsAuthenticationService;
            protected IOrganisationsServiceAgent OrganisationsServiceAgent;

            [SetUp]
            public virtual void SetUp()
            {
                FormsAuthenticationService = Substitute.For<IFormsAuthenticationService>();
                OrganisationsServiceAgent = Substitute.For<IOrganisationsServiceAgent>();
                OrganisationService = new OrganisationService(FormsAuthenticationService,OrganisationsServiceAgent);
            }
        }

        [TestFixture]
        public class When_I_call_Get_with_valid_logged_in_organisation : Given_an_OrganisationService
        {
            private readonly Organisation _organisationFromServiceAgent = new Organisation();
            private Organisation _result;
            private const string OrganisationId = "organisationId";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                FormsAuthenticationService.GetLoggedInOrganisationId().Returns(OrganisationId);
                OrganisationsServiceAgent.Get(OrganisationId).Returns(_organisationFromServiceAgent);
                _result = OrganisationService.Get();
            }

            [Test]
            public void it_should_return_the_organisation_from_the_service_agent_for_logged_in_user()
            {
                _result.ShouldBe(_organisationFromServiceAgent);
            }
        }

        [TestFixture]
        public class When_I_call_Get_with_invalid_logged_in_organisation_details : Given_an_OrganisationService
        {
            private const string InvalidOrganisationId = "invalid";
            private OrganisationIdNotValidException _exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                FormsAuthenticationService.GetLoggedInOrganisationId().Returns(InvalidOrganisationId);
                OrganisationsServiceAgent.Get(InvalidOrganisationId)
                                         .Returns(x => { throw new ItemNotFoundException(); });
                try
                {
                    OrganisationService.Get();
                }
                catch (OrganisationIdNotValidException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_return_OrganisationIdNotValidException()
            {
                _exception.ShouldNotBe(null);
            }
        }
    }
}