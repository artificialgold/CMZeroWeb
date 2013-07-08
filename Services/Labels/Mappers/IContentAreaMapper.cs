using CMZero.API.Messages;
using CMZero.Web.Models;

namespace CMZero.Web.Services.Labels.Mappers
{
    public interface IContentAreaMapper
    {
        ContentAreaForDisplay Map(ContentArea contentArea);
    }
}