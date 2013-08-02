using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Applications;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Login;

namespace CMZero.Web.Services.ViewModelGetters
{
    public class DashboardViewModelGetter : IDashboardViewModelGetter
    {
        private readonly IApplicationService _applicationService;
        private readonly IFormsAuthenticationService _formsAuthenticationService;
        private readonly ILabelCollectionRetriever _labelCollectionRetriever;

        public DashboardViewModelGetter(IApplicationService applicationService, IFormsAuthenticationService formsAuthenticationService, ILabelCollectionRetriever labelCollectionRetriever)
        {
            _applicationService = applicationService;
            _formsAuthenticationService = formsAuthenticationService;
            _labelCollectionRetriever = labelCollectionRetriever;
        }

        public DashboardViewModel Get()
        {
            var organisationId = _formsAuthenticationService.GetLoggedInOrganisationId();

            var applications = _applicationService.GetByOrganisationId(organisationId);

            var labels = _labelCollectionRetriever.Get("DashboardPage");

            return new DashboardViewModel
                {
                    Applications = applications,
                    Labels = labels
                };
        }
    }
}