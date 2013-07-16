using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using CMZero.API.Messages;
using CMZero.API.ServiceAgent;

namespace ContentAreaCreationApp
{
    class Program
    {
        private static readonly string BaseUri = ConfigurationManager.AppSettings["BaseUri"];

        static readonly IContentAreasServiceAgent ContentAreasServiceAgent = new ContentAreasServiceAgent(BaseUri);
        static readonly IOrganisationsServiceAgent OrganisationsServiceAgent = new OrganisationsServiceAgent(BaseUri);
        static readonly IApplicationsServiceAgent ApplicationsServiceAgent = new ApplicationsServiceAgent(BaseUri);
        static readonly ICollectionServiceAgent CollectionServiceAgent = new CollectionsServiceAgent(BaseUri);

        static void Main(string[] args)
        {
            Console.WriteLine("Checking for existence of data and/or creating new");

            var organisation = GetOrganisation();

            var application = GetApplication(organisation);

            var collection = GetCollection(application, organisation);

            var areasAlreadyCreated = ContentAreasServiceAgent.GetByCollection(collection.Id);
            foreach (var contentArea in areasAlreadyCreated)
            {
                Console.WriteLine("found area : " + contentArea.Name);
            }

            CreateContentArea(application, collection, "MainBody", "HelloWorldFromScript", areasAlreadyCreated);


            Console.WriteLine("CollectionId = " + collection.Id);

            Console.WriteLine();
            Console.WriteLine("press Enter to continue");
            Console.ReadLine();
        }

        private static void CreateContentArea(
            Application application,
            Collection collection,
            string name,
            string content,
            IEnumerable<ContentArea> areasAlreadyCreated)
        {
            var exists = (from ca in areasAlreadyCreated where ca.Name == name select ca).Any();

            if (!exists)
            {
                ContentAreasServiceAgent.Post(
                    new ContentArea
                        {
                            Active = true,
                            ApplicationId = application.Id,
                            CollectionId = collection.Id,
                            Content = content,
                            ContentType = ContentAreaType.HtmlArea,
                            Name = name
                        });

                Console.WriteLine("Created content area with name : " + name);
            }
        }

        private static Collection GetCollection(Application application, Organisation organisation)
        {
            Collection collection;
            try
            {
                collection = CollectionServiceAgent.Get(ConfigurationManager.AppSettings["CollectionIdToTryToUse"]);
                Console.WriteLine("Collection with that Id does exist");
                Console.WriteLine();
            }
            catch
            {
                collection =
                    CollectionServiceAgent.Post(
                        new Collection
                            {
                                Active = true,
                                ApplicationId = application.Id,
                                Name = "Home Page",
                                OrganisationId = organisation.Id
                            });
                Console.WriteLine("CREATED NEW COLLECTION FOR HOME PAGE : ID = " + collection.Id);
                Console.WriteLine();
            }

            if (collection == null)
            {
                Console.WriteLine("ERROR CREATING COLLECTION, process aborted");
                Console.ReadLine();
                return null;
            }
            return collection;
        }

        private static Application GetApplication(Organisation organisation)
        {
            Application application;
            try
            {
                application = ApplicationsServiceAgent.Get(ConfigurationManager.AppSettings["ApplicationIdToTryToUse"]);
                Console.WriteLine("Application with that ID does exist");
            }
            catch
            {
                application =
                    ApplicationsServiceAgent.Post(
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

        private static Organisation GetOrganisation()
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
