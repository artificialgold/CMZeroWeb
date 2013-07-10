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
        private static string baseUri = ConfigurationManager.AppSettings["BaseUri"];

        static IContentAreasServiceAgent _contentAreasServiceAgent = new ContentAreasServiceAgent(baseUri);
        static IOrganisationsServiceAgent _organisationsServiceAgent = new OrganisationsServiceAgent(baseUri);
        static IApplicationsServiceAgent _applicationsServiceAgent = new ApplicationsServiceAgent(baseUri);
        static ICollectionServiceAgent _collectionServiceAgent = new CollectionsServiceAgent(baseUri);

        static void Main(string[] args)
        {
            Console.WriteLine("Create Application");

            var organisation = GetOrganisation();

            var application = GetApplication(organisation);

            var collection = GetCollection(application, organisation);

            var areasAlreadyCreated = _contentAreasServiceAgent.GetByCollection(collection.Id);
            foreach (var contentArea in areasAlreadyCreated)
            {
                Console.WriteLine("found area : " + contentArea.Name);
            }

            CreateContentArea(application, collection, "MainBody", "HelloWorldFromScript", areasAlreadyCreated);


            Console.WriteLine("CollectionId = " + collection.Id);

            Console.WriteLine();
            Console.WriteLine("press any key to continue");
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
                _contentAreasServiceAgent.Post(
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
                collection = _collectionServiceAgent.Get(ConfigurationManager.AppSettings["CollectionIdToTryToUse"]);
            }
            catch
            {
                collection =
                    _collectionServiceAgent.Post(
                        new Collection
                            {
                                Active = true,
                                ApplicationId = application.Id,
                                Name = "Home Page",
                                OrganisationId = organisation.Id
                            });
                Console.WriteLine("CREATED NEW COLLECTION FOR HOME PAGE : ID = " + collection.Id);
            }

            if (collection == null)
            {
                Console.WriteLine("ERROR CREATING COLLECTION, process aborted");
                Console.ReadLine();
            }
            return collection;
        }

        private static Application GetApplication(Organisation organisation)
        {
            Application application;
            try
            {
                application = _applicationsServiceAgent.Get(ConfigurationManager.AppSettings["ApplicationIdToTryToUse"]);
            }
            catch
            {
                application =
                    _applicationsServiceAgent.Post(
                        new Application { Active = true, Name = "CMZero Website", OrganisationId = organisation.Id });
                Console.Write("APPLICATION WAS CREATED: ID = " + application.Id);
            }
            if (application == null)
            {
                Console.WriteLine("ERROR CREATING APPLICATION, process aborted");
                Console.ReadLine();
                return application;
            }
            return application;
        }

        private static Organisation GetOrganisation()
        {
            Organisation organisation;
            try
            {
                organisation = _organisationsServiceAgent.Get(ConfigurationManager.AppSettings["OrganisationIdToTryToUse"]);
            }
            catch (Exception)
            {
                organisation = _organisationsServiceAgent.Post(new Organisation { Active = true, Name = "CMZero2" });
                Console.WriteLine("NEW ORGANISATIONID IS " + organisation.Id);
            }

            if (organisation == null)
            {
                Console.WriteLine("ERROR CREATING ORGANISATION, process aborted");
                Console.ReadLine();
                return organisation;
            }
            return organisation;
        }
    }
}
