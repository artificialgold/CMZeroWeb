using System;
using System.Collections.Generic;
using System.Linq;
using CMZero.API.Messages;
using CMZero.API.ServiceAgent;
using CMZero.Web.Models.Exceptions;
using CMZero.Web.Services.Login;
using OrganisationIdNotValidException = CMZero.API.Messages.Exceptions.Organisations.OrganisationIdNotValidException;

namespace CMZero.Web.Services.Applications
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationsServiceAgent _applicationServiceAgent;
        private readonly IFormsAuthenticationService _formsAuthenticationService;

        public ApplicationService(IApplicationsServiceAgent applicationServiceAgent, IFormsAuthenticationService formsAuthenticationService)
        {
            _applicationServiceAgent = applicationServiceAgent;
            _formsAuthenticationService = formsAuthenticationService;
        }

        public IEnumerable<Application> GetByOrganisationId()
        {
            try
            {
                var organisationId = _formsAuthenticationService.GetLoggedInOrganisationId();
                return _applicationServiceAgent.GetByOrganisation(organisationId);
            }
            catch (OrganisationIdNotValidException)
            {
                throw new Models.Exceptions.OrganisationIdNotValidException();
            }
        }

        public Application GetById(string id)
        {
            var applicationsForOrganisation = GetByOrganisationId().ToArray();

            var applicationsWithId = CheckApplicationExistsForOrganisation(id, applicationsForOrganisation);
            var application = applicationsWithId.First();
            return application;
        }

        private static Application[] CheckApplicationExistsForOrganisation(string id, Application[] applicationsForOrganisation)
        {
            var applicationsWithId = (from a in applicationsForOrganisation where a.Id == id select a).ToArray();
            var applicationExistsForOrganisation = applicationsWithId.Any();

            if (!applicationExistsForOrganisation)
            {
                throw new ApplicationNotPartOfOrganisationException();
            }
            return applicationsWithId;
        }

        public Application Update(string id, string name)
        {
            GetById(id);

            throw new NotImplementedException();
        }
    }
}