using System.Collections.Generic;
using CMZero.API.Messages;
using CMZero.API.ServiceAgent;
using CMZero.Web.Services.Applications;

namespace CMZero.Web.Services.Collections
{
    public class CollectionService : ICollectionService
    {
        private readonly ICollectionServiceAgent _collectionsServiceAgent;
        private readonly IApplicationService _applicationService;

        public CollectionService(ICollectionServiceAgent collectionsServiceAgent, IApplicationService applicationService)
        {
            _collectionsServiceAgent = collectionsServiceAgent;
            _applicationService = applicationService;
        }

        public IEnumerable<Collection> GetByApplication(string applicationId)
        {
            return _collectionsServiceAgent.GetByApiKey(_applicationService.GetById(applicationId).ApiKey);
        }
    }
}