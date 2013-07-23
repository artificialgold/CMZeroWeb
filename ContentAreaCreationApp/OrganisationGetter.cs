using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMZero.API.Messages;
using CMZero.API.ServiceAgent;

namespace ContentAreaCreationApp
{
    class OrganisationGetter
    {
        private readonly IOrganisationsServiceAgent OrganisationsServiceAgent;

        public OrganisationGetter(IOrganisationsServiceAgent organisationsServiceAgent)
        {
            OrganisationsServiceAgent = organisationsServiceAgent;
        }

        public Organisation GetOrganisation()
        {
            Organisation organisation;
            try
            {
                organisation = OrganisationsServiceAgent.Get(ConfigurationManager.AppSettings["OrganisationIdToTryToUse"]);
                Console.WriteLine("Organisation with that ID does exist");
                Console.WriteLine();
            }
            catch (Exception)
            {
                organisation = OrganisationsServiceAgent.Post(new Organisation { Active = true, Name = "CMZero2" });
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
