using System;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Organisations;
using CMZero.API.ServiceAgent;
using CMZero.Web.Services.Login;

namespace CMZero.Web.Services.Organisations
{
    public class OrganisationService : IOrganisationService
    {
        private readonly IFormsAuthenticationService _formsAuthenticationService;
        private readonly IOrganisationsServiceAgent _organisationsServiceAgent;

        public OrganisationService(IFormsAuthenticationService formsAuthenticationService, IOrganisationsServiceAgent organisationsServiceAgent)
        {
            _formsAuthenticationService = formsAuthenticationService;
            _organisationsServiceAgent = organisationsServiceAgent;
        }

        public Organisation Get()
        {
            try
            {
                return _organisationsServiceAgent.Get(_formsAuthenticationService.GetLoggedInOrganisationId());
            }
            catch (ItemNotFoundException exception)
            {
                throw new OrganisationIdNotValidException();
            }
        }
    }
}