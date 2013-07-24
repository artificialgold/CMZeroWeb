using System.Collections.Generic;
using System.Linq;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.ApiKeys;
using CMZero.API.ServiceAgent;
using CMZero.Web.Models;
using CMZero.Web.Services;
using CMZero.Web.Services.Labels;
using CMZero.Web.Services.Labels.Mappers;
using CMZero.Web.Services.Logging;
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
            protected ILogger Logger;

            [SetUp]
            public virtual void SetUp()
            {
                LabelCollectionMapper = Substitute.For<ILabelCollectionMapper>();
                ContentAreasServiceAgent = Substitute.For<IContentAreasServiceAgent>();
                SystemSettings = Substitute.For<ISystemSettings>();
                Logger = Substitute.For<ILogger>();
                LabelCollectionRetriever = new LabelCollectionRetriever(LabelCollectionMapper, ContentAreasServiceAgent, SystemSettings, Logger);
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

        [TestFixture]
        public class When_I_call_Get_and_labelcollectionretriever_throws_an_error : Given_a_LabelCollectionRetriever
        {
            private const string ApiKey = "test";
            private const string CollectionName = "collectionName";
            private LabelCollection _result;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                ContentAreasServiceAgent.GetByCollectionNameAndApiKey(ApiKey, CollectionName)
                                        .Returns(x => { throw new ApiKeyNotValidException(); });
                SystemSettings.ApiKey.Returns(ApiKey);
                _result = LabelCollectionRetriever.Get(CollectionName);
            }

            [Test]
            public void it_should_return_empty_collection()
            {
                _result.ContentAreas.Count().ShouldBe(0);
            }

            [Test]
            public void it_should_call_logger()
            {
                Logger.ReceivedCalls().Count().ShouldBe(1);
            }
        }
    }
}