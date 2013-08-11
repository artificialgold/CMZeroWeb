using System;
using System.Collections.Generic;
using System.Linq;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.Web.Models.Exceptions;
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
            var application = GetApplication(applicationId);

            var labels = _labelCollectionRetriever.Get("ApplicationPage");

            return new ApplicationViewModel
                {
                    Application = application,
                    Labels = labels
                };
        }

        private Application GetApplication(string applicationId)
        {
            string organisationId = _formsAuthenticationService.GetLoggedInOrganisationId();
            var applicationsForOrganisation = _applicationService.GetByOrganisationId(organisationId).ToArray();

            IEnumerable<Application> applicationsWithId = from a in applicationsForOrganisation
                                                          where a.Id == applicationId
                                                          select a;
            var applicationExistsForOrganisation = applicationsWithId.Any();

            if (!applicationExistsForOrganisation) throw new ApplicationNotPartOfOrganisationException();
            var application = applicationsWithId.First();
            return application;
        }
    }
}