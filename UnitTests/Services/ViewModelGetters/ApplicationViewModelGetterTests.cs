using CMZero.Web.Models;
using CMZero.Web.Models.Exceptions;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Applications;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Login;
using CMZero.Web.Services.ViewModelGetters;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using CMZero.API.Messages;

namespace CMZero.Web.UnitTests.Services.ViewModelGetters
{
    public class ApplicationViewModelGetterTests
    {
        public class Given_a_ApplicationViewModelGetter
        {
            protected ApplicationViewModelGetter ApplicationViewModelGetter;
            protected IApplicationService ApplicationService;
            protected IFormsAuthenticationService FormsAuthenticationService;
            protected ILabelCollectionRetriever LabelCollectionRetriever;

            [SetUp]
            public virtual void SetUp()
            {
                ApplicationService = Substitute.For<IApplicationService>();
                FormsAuthenticationService = Substitute.For<IFormsAuthenticationService>();
                LabelCollectionRetriever = Substitute.For<ILabelCollectionRetriever>();
                ApplicationViewModelGetter = new ApplicationViewModelGetter(FormsAuthenticationService, ApplicationService, LabelCollectionRetriever);
            }
        }

        [TestFixture]
        public class When_I_call_Get_with_a_invalid_applicationId_for_this_organisation : Given_a_ApplicationViewModelGetter
        {
            private const string ApplicationIdThatDoesNotExist = "IDoNotExist";
            private ApplicationNotPartOfOrganisationException _exception;
            private const string OrganisationIdFromFormsAuthenticationService = "fromAuthService";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                FormsAuthenticationService.GetLoggedInOrganisationId()
                                          .Returns(OrganisationIdFromFormsAuthenticationService);
                ApplicationService.GetByOrganisationId(OrganisationIdFromFormsAuthenticationService)
                                      .Returns(new List<Application>());
                try
                {
                    ApplicationViewModelGetter.Get(ApplicationIdThatDoesNotExist);
                }
                catch (ApplicationNotPartOfOrganisationException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_return_ApplicationIdNotValidException()
            {
                _exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_Get_with_valid_applicationId : Given_a_ApplicationViewModelGetter
        {
            private ApplicationViewModel _result;
            private Application _applicationFromService;
            private LabelCollection _labelsFromRetreiever=new LabelCollection();
            private const string OrganisationIdFromAuthService = "orgIdFromAuthService";
            private const string ValidApplicationId = "iExist";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                LabelCollectionRetriever.Get("ApplicationPage").Returns(_labelsFromRetreiever);
                FormsAuthenticationService.GetLoggedInOrganisationId().Returns(OrganisationIdFromAuthService);
                _applicationFromService = new Application {Id = ValidApplicationId};
                ApplicationService.GetByOrganisationId(OrganisationIdFromAuthService)
                                  .Returns(new List<Application> {_applicationFromService});
                _result = ApplicationViewModelGetter.Get(ValidApplicationId);
            }

            [Test]
            public void it_should_return_model_with_application_from_service()
            {
                _result.Application.ShouldBe(_applicationFromService);
            }

            [Test]
            public void it_should_return_labels_from_retriever()
            {
                _result.Labels.ShouldBe(_labelsFromRetreiever);
            }
        }
    }
}