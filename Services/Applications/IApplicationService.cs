using System.Collections.Generic;
using CMZero.API.Messages;

namespace CMZero.Web.Services.Applications
{
    public interface IApplicationService
    {
        IEnumerable<Application> GetByOrganisationId();

        Application GetById(string id);
        Application Update(string applicationId, string name, bool active);
    }
}