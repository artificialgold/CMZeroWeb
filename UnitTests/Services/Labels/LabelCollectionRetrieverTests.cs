using CMZero.Web.Services.Labels;

using NUnit.Framework;

namespace CMZero.Web.UnitTests.Services.Labels
{
    [TestFixture]
    public class LabelCollectionRetreiverTests
    {
        public class Given_a_LabelCollectionRetriever
        {
            protected LabelCollectionRetriever labelCollectionRetreiver;

            [SetUp]
            public virtual void SetUp()
            {
                labelCollectionRetreiver = new LabelCollectionRetriever();
            }
        }


        [TestFixture]
        public class When_I_get_a_collection_by_valid_name_and_application : Given_a_LabelCollectionRetriever
        {
            [Test]
            public void it_should_return_collection_from_service_agent()
            {
                //result.ShouldBe(mappedValueFromServiceAgent);
            }
        }
    }
}