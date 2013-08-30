using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Applications;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Organisations;

namespace CMZero.Web.Services.ViewModelGetters
{
    public class DashboardViewModelGetter : IDashboardViewModelGetter
    {
        private readonly IApplicationService _applicationService;
        private readonly ILabelCollectionRetriever _labelCollectionRetriever;
        private readonly IOrganisationService _organisationService;

        public DashboardViewModelGetter(IApplicationService applicationService, ILabelCollectionRetriever labelCollectionRetriever, IOrganisationService organisationService)
        {
            _applicationService = applicationService;
            _labelCollectionRetriever = labelCollectionRetriever;
            _organisationService = organisationService;
        }

        public DashboardViewModel Get()
        {
            var organisation = _organisationService.Get();

            var applications = _applicationService.GetByOrganisationId();

            var labels = _labelCollectionRetriever.Get("DashboardPage");

            return new DashboardViewModel
                {
                    Organisation = organisation,
                    Applications = applications,
                    Labels = labels
                };
        }
    }
}