using System;
using System.Configuration;
using CMZero.API.Messages;
using CMZero.API.ServiceAgent;

namespace ContentAreaCreationApp
{
    class OrganisationGetter
    {
        private readonly IOrganisationsServiceAgent _organisationsServiceAgent;

        public OrganisationGetter(IOrganisationsServiceAgent organisationsServiceAgent)
        {
            _organisationsServiceAgent = organisationsServiceAgent;
        }

        public Organisation GetOrganisation()
        {
            Organisation organisation;
            try
            {
                organisation = _organisationsServiceAgent.Get(ConfigurationManager.AppSettings["OrganisationIdToTryToUse"]);
                Console.WriteLine("Organisation with that ID does exist");
                Console.WriteLine();
            }
            catch (Exception)
            {
                organisation = _organisationsServiceAgent.Post(new Organisation { Active = true, Name = "CMZeroWebsiteLabels" });
                Console.WriteLine("NEW ORGANISATIONID IS " + organisation.Id);
                Console.WriteLine();
            }

            if (organisation == null)
            {
                Console.WriteLine("ERROR CREATING ORGANISATION, process aborted");
                Console.ReadLine();
                return null;
            }
            return organisation;
        }
    }
}
