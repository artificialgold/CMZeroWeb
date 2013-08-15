using CMZero.API.Messages.Exceptions.Applications;
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
                ApplicationViewModelGetter = new ApplicationViewModelGetter(ApplicationService, LabelCollectionRetriever);
            }
        }

        [TestFixture]
        public class When_I_call_Get_with_a_invalid_applicationId_for_this_organisation : Given_a_ApplicationViewModelGetter
        {
            private const string ApplicationIdThatDoesNotExist = "IDoNotExist";
            private ApplicationNotPartOfOrganisationException _exception;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationService.GetById(ApplicationIdThatDoesNotExist)
                                      .Returns(x => { throw new ApplicationNotPartOfOrganisationException(); });
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
            private LabelCollection _labelsFromRetreiever = new LabelCollection();
            private const string ValidApplicationId = "iExist";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                LabelCollectionRetriever.Get("ApplicationPage").Returns(_labelsFromRetreiever);
                _applicationFromService = new Application { Id = ValidApplicationId };
                ApplicationService.GetById(ValidApplicationId)
                                  .Returns(_applicationFromService);
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

        [TestFixture]
        public class When_I_call_Update_with_an_applicationId_not_for_logged_in_organisation : Given_a_ApplicationViewModelGetter
        {
            private ApplicationNotPartOfOrganisationException _exception;
            private const string ApplicationIdNotPartOfOrganisation = "appIdNotPartOfOrg";
            private const string NewName = "name";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                try
                {
                    ApplicationService.Update(ApplicationIdNotPartOfOrganisation, NewName)
                                      .Returns(x => { throw new ApplicationNotPartOfOrganisationException(); });
                    ApplicationViewModelGetter.Update(ApplicationIdNotPartOfOrganisation, NewName);
                }
                catch (ApplicationNotPartOfOrganisationException ex)
                {
                    _exception = ex;
                }
            }

            [Test]
            public void it_should_throw_ApplicationNotPartOfOrganistionException()
            {
                _exception.ShouldNotBe(null);
            }
        }

        [TestFixture]
        public class When_I_call_update_with_an_application_name_that_exists_for_organisation : Given_a_ApplicationViewModelGetter
        {
            private ApplicationViewModel _result;
            private readonly Application _applicationFromGetById = new Application();
            private const string ApplicationId = "applicationId";
            private const string NewName = "newName";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationService.Update(ApplicationId, NewName)
                                  .Returns(x => { throw new ApplicationNameAlreadyExistsException(); });
                ApplicationService.GetById(ApplicationId).Returns(_applicationFromGetById);
                _result = ApplicationViewModelGetter.Update(ApplicationId, NewName);
            }

            [Test]
            public void it_should_return_view_model_with_success_false()
            {
                _result.Success.ShouldBe(false);
            }

            [Test]
            public void it_should_return_view_model_with_name_exists_message_type()
            {
                _result.SuccessMessage.ShouldBe(SuccessMessages.ApplicationNameAlreadyExists);
            }

            [Test]
            public void it_should_return_application_from_service_agent()
            {
                _result.Application.ShouldBe(_applicationFromGetById);
            }
        }

        [TestFixture]
        public class When_I_call_Update_with_valid_parameters : Given_a_ApplicationViewModelGetter
        {
            private readonly Application _applicationFromService=new Application();
            private ApplicationViewModel _result;
            private const string NewName = "newName";
            private const string ApplicationId = "applicationId";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationService.Update(ApplicationId, NewName).Returns(_applicationFromService);
                _result = ApplicationViewModelGetter.Update(ApplicationId, NewName);
            }

            [Test]
            public void it_should_return_application_from_service()
            {
                _result.Application.ShouldBe(_applicationFromService);
            }

            [Test]
            public void it_should_return_success_true()
            {
                _result.Success.ShouldBe(true);
            }

            [Test]
            public void it_should_return_success_message_success()
            {
                _result.SuccessMessage.ShouldBe(SuccessMessages.Success);
            }
        }
    }
}