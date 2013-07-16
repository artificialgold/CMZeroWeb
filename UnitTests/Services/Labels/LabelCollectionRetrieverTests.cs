using System.Collections.Generic;

using CMZero.API.Messages;
using CMZero.API.ServiceAgent;
using CMZero.Web.Models;
using CMZero.Web.Services;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Labels.Mappers;

using NSubstitute;

using NUnit.Framework;

using Shouldly;

namespace CMZero.Web.UnitTests.Services.Labels
{
    [TestFixture]
    public class LabelCollectionRetreiverTests
    {
        public class Given_a_LabelCollectionRetriever
        {
            protected LabelCollectionRetriever LabelCollectionRetriever;

            protected ILabelCollectionMapper LabelCollectionMapper;

            protected IContentAreasServiceAgent ContentAreasServiceAgent;

            protected ISystemSettings SystemSettings;

            [SetUp]
            public virtual void SetUp()
            {
                LabelCollectionMapper = Substitute.For<ILabelCollectionMapper>();
                ContentAreasServiceAgent = Substitute.For<IContentAreasServiceAgent>();
                SystemSettings = Substitute.For<ISystemSettings>();
                LabelCollectionRetriever = new LabelCollectionRetriever(LabelCollectionMapper, ContentAreasServiceAgent, SystemSettings);
            }
        }

        [TestFixture]
        public class When_I_get_a_collection_by_valid_name_and_apiKey : Given_a_LabelCollectionRetriever
        {
            private const string CollectionName = "test";

            private LabelCollection result;

            private readonly LabelCollection mappedValueFromServiceAgent = new LabelCollection();

            private List<ContentArea> contentAreas = new List<ContentArea>();

            private const string ApiKey = "apiKey";

            [SetUp]
            public new virtual void SetUp()
            {
                SystemSettings.ApiKey.Returns(ApiKey);
                ContentAreasServiceAgent.GetByCollectionNameAndApiKey(ApiKey, CollectionName).Returns(contentAreas);
                LabelCollectionMapper.Map(contentAreas).Returns(mappedValueFromServiceAgent);
                result = LabelCollectionRetriever.Get(CollectionName);
            }

            [Test]
            public void it_should_return_collection_from_service_agent()
            {
                result.ShouldBe(mappedValueFromServiceAgent);
            }
        }
    }
}