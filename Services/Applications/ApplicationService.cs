using System.Collections.Generic;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Organisations;
using CMZero.API.ServiceAgent;

namespace CMZero.Web.Services.Applications
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationsServiceAgent _applicationServiceAgent;

        public ApplicationService(IApplicationsServiceAgent applicationServiceAgent)
        {
            _applicationServiceAgent = applicationServiceAgent;
        }

        public IEnumerable<Application> GetByOrganisationId(string organisationId)
        {
            try
            {
                return _applicationServiceAgent.GetByOrganisation(organisationId);
            }
            catch (OrganisationIdNotValidException)
            {
                throw new Models.Exceptions.OrganisationIdNotValidException();
            }
        }

        public Application GetById(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}