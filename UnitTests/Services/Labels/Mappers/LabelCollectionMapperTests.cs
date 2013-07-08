using System.Collections.Generic;

using CMZero.API.Messages;
using CMZero.Web.Models;
using CMZero.Web.Services.Labels.Mappers;

using NSubstitute;

using NUnit.Framework;

using Shouldly;

namespace CMZero.Web.UnitTests.Services.Labels.Mappers
{
    public class LabelCollectionMapperTests
    {
        public class Given_a_LabelCollectionMapper
        {
            protected LabelCollectionMapper LabelCollectionMapper;

            protected IContentAreaMapper ContentAreaMapper;

            [SetUp]
            public virtual void SetUp()
            {
                ContentAreaMapper = Substitute.For<IContentAreaMapper>();
                LabelCollectionMapper = new LabelCollectionMapper(ContentAreaMapper);
            }
        }

        [TestFixture]
        public class When_I_map_a_list_of_contentAreas : Given_a_LabelCollectionMapper
        {
            private IEnumerable<ContentArea> contentAreasToMap;

            private LabelCollection result;

            private ContentAreaForDisplay mappedContentArea1 = new ContentAreaForDisplay();
            private ContentAreaForDisplay mappedContentArea2 = new ContentAreaForDisplay();

            private ContentArea contentarea1 = new ContentArea();
            private ContentArea contentarea2 = new ContentArea();

            [SetUp]
            public new virtual void SetUp()
            {
                base.SetUp();
                contentAreasToMap = new List<ContentArea> { contentarea1, contentarea2 };
                ContentAreaMapper.Map(contentarea1).Returns(mappedContentArea1);
                ContentAreaMapper.Map(contentarea2).Returns(mappedContentArea2);
                result = LabelCollectionMapper.Map(contentAreasToMap);
            }

            [Test]
            public void it_should_return_a_list_of_mapped_content_areas()
            {
                result.ContentAreas.ShouldContain(mappedContentArea1);
                result.ContentAreas.ShouldContain(mappedContentArea2);
            }
        }
    }
}