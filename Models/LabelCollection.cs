using System.Collections.Generic;

using CMZero.API.Messages;

namespace CMZero.Web.Models
{
    public class LabelCollection
    {
        public IEnumerable<ContentAreaForDisplay> ContentAreas { get; set; }
    }

    public class ContentAreaForDisplay
    {
        public string Name { get; set; }

        public string CollectionId { get; set; }

        public string Content { get; set; }
    }
}
