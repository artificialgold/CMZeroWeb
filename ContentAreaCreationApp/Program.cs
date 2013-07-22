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

            IEnumerable<Collection> collectionsAlreadyExisting = CollectionServiceAgent.GetByApiKey(application.ApiKey);

            GetHomePageContentAreas(application, collectionsAlreadyExisting);

            Console.WriteLine();
            Console.WriteLine("press Enter to continue");
            Console.ReadLine();
        }

        private static void GetHomePageContentAreas(Application application, IEnumerable<Collection> collectionsAlreadyExisting)
        {
            Dictionary<string, string> contentRequired = new Dictionary<string, string>
                {
                    {"MainBody", "HelloWorldFromScript"},
                    {"PageTitle", "CMZero - Small chunk content management system"},
                    {"MainH1", "Content management for smaller chunks"}
                };

            CreateCollectionAndContentAreas(application, "Home Page", contentRequired, collectionsAlreadyExisting);
        }

        private static void CreateCollectionAndContentAreas(Application application, string collectionName, Dictionary<string, string> contentRequired, IEnumerable<Collection> collections)
        {
            var collection = CreateCollection(collectionName, application, collections);
            var areasAlreadyCreated = AreasAlreadyCreated(application.ApiKey, collectionName);

            foreach (var content in contentRequired)
            {
                CreateContentArea(application.Id, collection.Id, content.Key, content.Value, areasAlreadyCreated);
            }
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

        private static Collection CreateCollection(string name, Application application, IEnumerable<Collection> collections)
        {
            Collection collection;

            bool exists = (from c in collections
                           where c.Name == name
                           select c).Any();

            if (exists)
            {
                collection = (from c in collections
                              where c.Name == name
                              select c).First();
                Console.WriteLine("Collection with that Id does exist");
                Console.WriteLine();
            }
            else
            {
                collection =
                    CollectionServiceAgent.Post(
                        new Collection
                            {
                                Active = true,
                                ApplicationId = application.Id,
                                Name = name,
                                OrganisationId = application.OrganisationId
                            });
                Console.WriteLine("CREATED NEW COLLECTION WITH NAME: " + name);
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
