using CMZero.API.Messages;
using CMZero.Web.Models;
using CMZero.Web.Services.Labels.Mappers;

using NUnit.Framework;

using Shouldly;

namespace CMZero.Web.UnitTests.Services.Labels.Mappers
{
    public class ContentAreaMapperTests
    {
        public class Given_a_content_area_mapper
        {
            protected ContentAreaMapper ContentAreaMapper;

            [SetUp]
            public virtual void SetUp()
            {
                ContentAreaMapper = new ContentAreaMapper();
            }
        }


        [TestFixture]
        public class When_I_map_a_content_area : Given_a_content_area_mapper
        {
            private ContentArea contentArea = new ContentArea{Name = name, CollectionId = collectionId, Content = content};

            private static string name="name";

            private static string collectionId="collectionId";

            private static string content="contentkjhkjhkljhlkj";

            private ContentAreaForDisplay result;

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                result = ContentAreaMapper.Map(contentArea);
            }

            [Test]
            public void it_should_map_name_value()
            {
                result.Name.ShouldBe(name);
            }

            [Test]
            public void it_should_map_collectionId_value()
            {
                result.CollectionId.ShouldBe(collectionId);
            }

            [Test]
            public void it_should_map_content_value()
            {
                result.Content.ShouldBe(content);
            }
        }

    }
}