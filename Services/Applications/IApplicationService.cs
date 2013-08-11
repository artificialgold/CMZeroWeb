using System.Collections.Generic;
using CMZero.API.Messages;

namespace CMZero.Web.Services.Applications
{
    public interface IApplicationService
    {
        IEnumerable<Application> GetByOrganisationId(string organisationId);
    }
}