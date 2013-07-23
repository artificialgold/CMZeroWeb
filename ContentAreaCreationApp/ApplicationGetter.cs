using System;
using System.Configuration;
using CMZero.API.Messages;
using CMZero.API.ServiceAgent;

namespace ContentAreaCreationApp
{
    class ApplicationGetter
    {
        private readonly IApplicationsServiceAgent _applicationsServiceAgent;

        public ApplicationGetter(IApplicationsServiceAgent applicationsServiceAgent)
        {
            _applicationsServiceAgent = applicationsServiceAgent;
        }

        public Application GetApplication(Organisation organisation)
        {
            Application application;
            try
            {
                application = _applicationsServiceAgent.Get(ConfigurationManager.AppSettings["ApplicationIdToTryToUse"]);
                Console.WriteLine("Application with that ID does exist");
            }
            catch
            {
                application =
                    _applicationsServiceAgent.Post(
                        new Application { Active = true, Name = "CMZero Website", OrganisationId = organisation.Id });
                Console.WriteLine("APPLICATION WAS CREATED: ID = " + application.Id);
                Console.WriteLine("APIKEY = " + application.ApiKey);
                Console.WriteLine();
            }
            if (application == null)
            {
                Console.WriteLine("ERROR CREATING APPLICATION, process aborted");
                Console.ReadLine();
                return null;
            }
            return application;
        }
    }
}