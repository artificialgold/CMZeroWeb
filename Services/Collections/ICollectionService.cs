using System.Collections.Generic;
using CMZero.API.Messages;

namespace CMZero.Web.Services.Collections
{
    public interface ICollectionService
    {
        IEnumerable<Collection> GetByApplication(string applicationId);
    }
}