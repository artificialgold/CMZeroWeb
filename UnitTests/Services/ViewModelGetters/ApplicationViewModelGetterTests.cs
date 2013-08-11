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
            private const string ApplicationIdNotPartOfOrganisation = "appIdNotPartOfOrg";

            private const string NewName = "name";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                //ApplicationService.GetById(ApplicationIdNotPartOfOrganisation).Returns()
                ApplicationViewModelGetter.Update(ApplicationIdNotPartOfOrganisation, NewName);
            }
        }
    }
}