using CMZero.API.Messages;
using CMZero.Web.Models;

namespace CMZero.Web.Services.Labels.Mappers
{
    public class ContentAreaMapper : IContentAreaMapper
    {
        public ContentAreaForDisplay Map(ContentArea contentArea)
        {
            return new ContentAreaForDisplay
                       {
                           Name = contentArea.Name,
                           CollectionId = contentArea.CollectionId,
                           Content = contentArea.Content
                       };
        }
    }
}