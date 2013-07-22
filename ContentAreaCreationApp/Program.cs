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

            //TODO: Figure out how to stop this step. We need to create the collection if it does not exist but 
            var collectionHomePage = GetHomePageCollection(application, organisation);

            CreateHomePageCollection(application, collectionHomePage);
            CreateNavBarCollection(application, );

            Console.WriteLine();
            Console.WriteLine("press Enter to continue");
            Console.ReadLine();
        }

        private static void CreateHomePageCollection(Application application, Collection collection)
        {
            var areasAlreadyCreated = AreasAlreadyCreated(application.ApiKey, "Home Page");

            CreateContentArea(application.Id, collection.Id, "MainBody", "HelloWorldFromScript",
                              areasAlreadyCreated);
            CreateContentArea(application.Id, collection.Id, "PageTitle",
                              "CMZero - Small chunk content management system", areasAlreadyCreated);
            CreateContentArea(application.Id, collection.Id, "MainH1", "Content management for smaller chunks",
                              areasAlreadyCreated);
        }

        private static void CreateNavBarCollection(Application application, Collection collection)
        {
            var areasAlreadyCreated = AreasAlreadyCreated(application.ApiKey, "Home Page");

            CreateContentArea(application.Id, collection.Id, "MainBody", "HelloWorldFromScript",
                              areasAlreadyCreated);
            CreateContentArea(application.Id, collection.Id, "PageTitle",
                              "CMZero - Small chunk content management system", areasAlreadyCreated);
            CreateContentArea(application.Id, collection.Id, "MainH1", "Content management for smaller chunks",
                              areasAlreadyCreated);
        }

        private static IEnumerable<ContentArea> AreasAlreadyCreated(string apiKey, string collectionName)
        {
            var areasAlreadyCreated = ContentAreasServiceAgent.GetByCollectionNameAndApiKey(apiKey, collectionName);
            Console.WriteLine("Areas for - {0} - collection", collectionName);
            return areasAlreadyCreated;
        }

        private static void CreateContentArea(
            string applicationId,
            string collectionId,
            string name,
            string content,
            IEnumerable<ContentArea> areasAlreadyCreated)
        {
            var exists = (from ca in areasAlreadyCreated where ca.Name == name select ca).Any();

            if (exists) Console.WriteLine("found area : " + name);
            else
            {
                ContentAreasServiceAgent.Post(
                    new ContentArea
                        {
                            Active = true,
                            ApplicationId = applicationId,
                            CollectionId = collectionId,
                            Content = content,
                            ContentType = ContentAreaType.HtmlArea,
                            Name = name
                        });

                Console.WriteLine("Created content area with name : " + name);
            }
        }

        private static Collection GetHomePageCollection(Application application, Organisation organisation)
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
