using System.Collections.Generic;

using CMZero.API.Messages;
using CMZero.API.ServiceAgent;
using CMZero.Web.Models;
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

            [SetUp]
            public virtual void SetUp()
            {
                LabelCollectionMapper = Substitute.For<ILabelCollectionMapper>();
                ContentAreasServiceAgent = Substitute.For<IContentAreasServiceAgent>();
                LabelCollectionRetriever = new LabelCollectionRetriever(LabelCollectionMapper, ContentAreasServiceAgent);
            }
        }

        [TestFixture]
        public class When_I_get_a_collection_by_valid_name_and_application : Given_a_LabelCollectionRetriever
        {
            private const string CollectionId = "test";

            private LabelCollection result;

            private readonly LabelCollection mappedValueFromServiceAgent = new LabelCollection();

            [SetUp]
            public new virtual void SetUp()
            {

                LabelCollectionMapper.Map(new List<ContentArea>()).Returns(mappedValueFromServiceAgent);
                ContentAreasServiceAgent.GetByCollection(CollectionId).Returns(new List<ContentArea>());
                result = LabelCollectionRetriever.Get(CollectionId);
            }

            [Test]
            public void it_should_return_collection_from_service_agent()
            {
                result.ShouldBe(mappedValueFromServiceAgent);
            }
        }
    }
}