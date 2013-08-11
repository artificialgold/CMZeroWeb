using System;

using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Applications;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Login;

namespace CMZero.Web.Services.ViewModelGetters
{
    public class ApplicationViewModelGetter : IApplicationViewModelGetter
    {
        private readonly IFormsAuthenticationService _formsAuthenticationService;
        private readonly IApplicationService _applicationService;
        private readonly ILabelCollectionRetriever _labelCollectionRetriever;

        public ApplicationViewModelGetter(IFormsAuthenticationService formsAuthenticationService, IApplicationService applicationService, ILabelCollectionRetriever labelCollectionRetriever)
        {
            _formsAuthenticationService = formsAuthenticationService;
            _applicationService = applicationService;
            _labelCollectionRetriever = labelCollectionRetriever;
        }

        public ApplicationViewModel Get(string applicationId)
        {
            var application = _applicationService.GetById(applicationId);

            var labels = _labelCollectionRetriever.Get("ApplicationPage");

            return new ApplicationViewModel
                {
                    Application = application,
                    Labels = labels
                };
        }

        public ApplicationViewModel Update(string applicationId, string name)
        {
            throw new NotImplementedException();
        }
    }
}