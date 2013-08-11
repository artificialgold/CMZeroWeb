using System.Collections.Generic;
using CMZero.API.Messages;
using CMZero.Web.Models;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Applications;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Login;
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
            protected IApplicationService ApplicationService;
            protected DashboardViewModelGetter DashboardViewModelGetter;
            protected IFormsAuthenticationService FormsAuthenticationService;
            protected ILabelCollectionRetriever LabelCollectionRetriever;

            [SetUp]
            public virtual void SetUp()
            {
                FormsAuthenticationService = Substitute.For<IFormsAuthenticationService>();
                ApplicationService = Substitute.For<IApplicationService>();
                LabelCollectionRetriever = Substitute.For<ILabelCollectionRetriever>();
                DashboardViewModelGetter = new DashboardViewModelGetter(ApplicationService, LabelCollectionRetriever);
            }
        }

        [TestFixture]
        public class When_I_call_Get : Given_a_DashboardViewModelGetter
        {
            private DashboardViewModel _result;
            private readonly IEnumerable<Application> _applicationsFromService = new List<Application>();
            private readonly LabelCollection _labelsFromCollectionRetriever = new LabelCollection();

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationService.GetByOrganisationId();
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
        }
    }
}