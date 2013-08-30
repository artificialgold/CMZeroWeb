using System.Collections.Generic;
using CMZero.API.Messages;
using CMZero.Web.Models;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Applications;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Login;
using CMZero.Web.Services.Organisations;
using CMZero.Web.Services.ViewModelGetters;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace CMZero.Web.UnitTests.Services.ViewModelGetters
{
    public class DashboardViewModelGetterTests
    {
        public class Given_a_DashboardViewModelGetter
        {
            protected IOrganisationService OrganisationService;
            protected IApplicationService ApplicationService;
            protected DashboardViewModelGetter DashboardViewModelGetter;
            protected IFormsAuthenticationService FormsAuthenticationService;
            protected ILabelCollectionRetriever LabelCollectionRetriever;

            [SetUp]
            public virtual void SetUp()
            {
                OrganisationService = Substitute.For<IOrganisationService>();
                FormsAuthenticationService = Substitute.For<IFormsAuthenticationService>();
                ApplicationService = Substitute.For<IApplicationService>();
                LabelCollectionRetriever = Substitute.For<ILabelCollectionRetriever>();
                DashboardViewModelGetter = new DashboardViewModelGetter(ApplicationService, LabelCollectionRetriever, OrganisationService);
            }
        }

        [TestFixture]
        public class When_I_call_Get : Given_a_DashboardViewModelGetter
        {
            private DashboardViewModel _result;
            private readonly IEnumerable<Application> _applicationsFromService = new List<Application>{new Application()};
            private readonly LabelCollection _labelsFromCollectionRetriever = new LabelCollection();
            private readonly Organisation _organisationFromService = new Organisation();

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationService.GetByOrganisationId().Returns(_applicationsFromService);
                OrganisationService.Get().Returns(_organisationFromService);
                LabelCollectionRetriever.Get("DashboardPage").Returns(_labelsFromCollectionRetriever);
                _result = DashboardViewModelGetter.Get();
            }

            [Test]
            public void it_should_call_application_service_to_get_all_applications_for_the_logged_in_organisationId()
            {
                _result.Applications.ShouldBe(_applicationsFromService);
            }

            [Test]
            public void it_should_return_labels_for_this_collection()
            {
                _result.Labels.ShouldBe(_labelsFromCollectionRetriever);
            }

            [Test]
            public void it_should_call_organisation_service_to_get_organisation_information()
            {
                _result.Organisation.ShouldBe(_organisationFromService);
            }
        }
    }
}