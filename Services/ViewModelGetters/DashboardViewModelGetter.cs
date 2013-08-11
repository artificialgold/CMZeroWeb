using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Applications;
using CMZero.Web.Services.Labels;

namespace CMZero.Web.Services.ViewModelGetters
{
    public class DashboardViewModelGetter : IDashboardViewModelGetter
    {
        private readonly IApplicationService _applicationService;
        private readonly ILabelCollectionRetriever _labelCollectionRetriever;

        public DashboardViewModelGetter(IApplicationService applicationService, ILabelCollectionRetriever labelCollectionRetriever)
        {
            _applicationService = applicationService;
            _labelCollectionRetriever = labelCollectionRetriever;
        }

        public DashboardViewModel Get()
        {
            var applications = _applicationService.GetByOrganisationId();

            var labels = _labelCollectionRetriever.Get("DashboardPage");

            return new DashboardViewModel
                {
                    Applications = applications,
                    Labels = labels
                };
        }
    }
}