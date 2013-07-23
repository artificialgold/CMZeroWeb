using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

using CMZero.API.Messages;
using CMZero.API.ServiceAgent;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            GetContentAreas("Home Page", application, collectionsAlreadyExisting);
            GetContentAreas("NavBar", application, collectionsAlreadyExisting);

            Console.WriteLine();
            Console.WriteLine("press Enter to continue");
            Console.ReadLine();
        }

        private static void GetContentAreas(string collectionName, Application application, IEnumerable<Collection> collectionsAlreadyExisting)
        {
            var stream = File.OpenText(string.Format("../../Collections/{0}.json", collectionName));
            JsonTextReader reader = new JsonTextReader(stream);

            Dictionary<string, string> contentRequired = new Dictionary<string, string>();

            Console.WriteLine("Read content from json file for Collection : {0}", collectionName);
            JArray root = JArray.Load(reader);
            foreach (JObject o in root)
            {
                contentRequired.Add(o["Name"].ToString(), o["Content"].ToString());
                Console.WriteLine("{0} : {1}", o["Name"], o["Content"]);
            }

            CreateCollectionAndContentAreas(application, collectionName, contentRequired, collectionsAlreadyExisting);
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
            return new ApplicationGetter(ApplicationsServiceAgent).GetApplication(organisation);
        }

        private static Organisation GetOrganisation()
        {
            return new OrganisationGetter(OrganisationsServiceAgent).GetOrganisation();
        }
    }
    public class BasicContentArea
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }

}
