using System.Collections.Generic;
using CMZero.API.Messages;
using CMZero.API.ServiceAgent;
using CMZero.Web.Services.Applications;
using CMZero.Web.Services.Collections;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace CMZero.Web.UnitTests.Services.Collections
{
    public class CollectionServiceTests
    {
        public class Given_a_CollectionService
        {
            protected CollectionService CollectionService;
            protected ICollectionServiceAgent CollectionsServiceAgent;
            protected IApplicationService ApplicationService;

            [SetUp]
            public virtual void SetUp()
            {
                ApplicationService = Substitute.For<IApplicationService>();
                CollectionsServiceAgent = Substitute.For<ICollectionServiceAgent>();
                CollectionService = new CollectionService(CollectionsServiceAgent, ApplicationService);
            }
        }

        [TestFixture]
        public class When_I_call_get_by_application_id_with_valid_applicationId : Given_a_CollectionService
        {
            private const string ApplicationId = "applicationId";
            private IEnumerable<Collection> _result;
            private readonly IEnumerable<Collection> _listFromServiceAgent = new List<Collection> { new Collection() };
            private const string ApiKey = "apiKey";

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ApplicationService.GetById(ApplicationId)
                                  .Returns(new Application {Id = ApplicationId, ApiKey = ApiKey});
                CollectionsServiceAgent.GetByApiKey(ApiKey).Returns(_listFromServiceAgent);
                _result = CollectionService.GetByApplication(ApplicationId);
            }

            [Test]
            public void it_should_return_collections_from_service_agent()
            {
                _result.ShouldBe(_listFromServiceAgent);
            }
        }
    }
}