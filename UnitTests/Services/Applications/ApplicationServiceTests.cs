using System;
using System.Collections.Generic;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.API.ServiceAgent;
using CMZero.Web.Models.Exceptions;
using CMZero.Web.Services.Applications;
using CMZero.Web.Services.Login;
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
            protected IFormsAuthenticationService FormsAuthenticationService;

            [SetUp]
            public void SetUp()
            {
                ApplicationServiceAgent = Substitute.For<IApplicationsServiceAgent>();
                FormsAuthenticationService = Substitute.For<IFormsAuthenticationService>();
                ApplicationService = new ApplicationService(ApplicationServiceAgent, FormsAuthenticationService);
            }
        }

        [TestFixture]
        public class When_I_call_GetByOrganisationId_with_invalid_organisationId
            : Given_an_ApplicationService
        {
            private OrganisationIdNotValidException _exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationService.GetByOrganisationId()
                                  .Returns(
                                      x =>
                                      {
                                          throw new API.Messages.Exceptions.Organisations.
                                              OrganisationIdNotValidException();
                                      });

                try
                {
                    ApplicationService.GetByOrganisationId();
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
                _result = ApplicationService.GetByOrganisationId();
            }

            [Test]
            public void it_should_return_the_result_from_service_agent()
            {
                _result.ShouldBe(_returnThis);
            }
        }

        [TestFixture]
        public class When_I_call_GetById_with_application_not_part_of_organisation : Given_an_ApplicationService
        {
            private const string ApplicationIdNotPartOfOrganisation = "appIdNotPartOfOrgId";
            private ApplicationNotPartOfOrganisationException _exception;
            private const string OrganisationId = "organisationId";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                try
                {
                    FormsAuthenticationService.GetLoggedInOrganisationId().Returns(OrganisationId);
                    ApplicationServiceAgent.GetByOrganisation(OrganisationId).Returns(new List<Application> { new Application { Id = ApplicationIdNotPartOfOrganisation + "xxx" } });
                    ApplicationService.GetById(ApplicationIdNotPartOfOrganisation);
                }
                catch (ApplicationNotPartOfOrganisationException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_return_ApplicationNotPartOfOrganisationException()
            {
                _exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_GetById_with_a_valid_applicationId : Given_an_ApplicationService
        {
            private const string OrganisationId = "organisationId";
            private const string ApplicationId = "applicationId";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                FormsAuthenticationService.GetLoggedInOrganisationId().Returns(OrganisationId);
                ApplicationServiceAgent.GetByOrganisation(OrganisationId)
                                       .Returns(new List<Application> { new Application { Id = ApplicationId } });
                ApplicationService.GetById(ApplicationId);
            }
        }

        [TestFixture]
        public class When_I_call_Update_with_applicationId_not_applicable_for_logged_organisation : Given_an_ApplicationService
        {
            private ApplicationNotPartOfOrganisationException _exception;

            private const string OrganisationId = "orgId";

            private const string ApplicationIdNotPartOfLoggedInOrganisation = "applicationIdNotValid";

            private const string NewName = "new";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                try
                {
                    FormsAuthenticationService.GetLoggedInOrganisationId().Returns(OrganisationId);
                    ApplicationServiceAgent.GetByOrganisation(OrganisationId).Returns(new List<Application>());
                    ApplicationService.Update(ApplicationIdNotPartOfLoggedInOrganisation, NewName);
                }
                catch (ApplicationNotPartOfOrganisationException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_return_AppliationNotPartOfOrganisationException()
            {
                _exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_Update_with_valid_parameters : Given_an_ApplicationService
        {
            private Application _result;

            private Application _applicationFromServiceAgentForGet;
            private const string OrganisationId = "organisationId";

            private const string NewName = "newName";

            private const string ApplicationId = "applicationId";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                FormsAuthenticationService.GetLoggedInOrganisationId().Returns(OrganisationId);
                _applicationFromServiceAgentForGet = new Application {Id = ApplicationId};
                ApplicationServiceAgent.GetByOrganisation(OrganisationId)
                                       .Returns(new List<Application> {_applicationFromServiceAgentForGet});
                ApplicationServiceAgent.Get(ApplicationId).Returns(_applicationFromServiceAgentForGet);
                _applicationFromServiceAgentForGet.Name = NewName;
                ApplicationServiceAgent.Put(_applicationFromServiceAgentForGet)
                                       .Returns(_applicationFromServiceAgentForGet);
                _result = ApplicationService.Update(ApplicationId, NewName);
            }

            [Test]
            public void it_should_return_value_from_service_agent()
            {
                _result.ShouldBe(_applicationFromServiceAgentForGet);
            }
        }

        [TestFixture]
        public class When_I_call_Update_with_application_name_that_already_exists : Given_an_ApplicationService
        {
            private Application _application;

            private ApplicationNameAlreadyExistsException _exception;

            private const string ApplicationId = "applicationId";

            private const string NewNameThatAlreadyExists = "existingName";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                const string organisationId = "orgId";
                FormsAuthenticationService.GetLoggedInOrganisationId().Returns(organisationId);
                _application = new Application { Id = ApplicationId , OrganisationId = organisationId};
                ApplicationServiceAgent.GetByOrganisation(organisationId)
                                       .Returns(new List<Application> { _application });
                _application.Name = NewNameThatAlreadyExists;
                ApplicationServiceAgent.Put(_application)
                                       .Returns(x => { throw new ApplicationNameAlreadyExistsException(); });
                try
                {
                    ApplicationService.Update(ApplicationId, NewNameThatAlreadyExists);
                }
                catch (ApplicationNameAlreadyExistsException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_return_ApplicationNameAlreadyExistsException()
            {
                _exception.ShouldNotBe(null);
            }
        }
    }
}