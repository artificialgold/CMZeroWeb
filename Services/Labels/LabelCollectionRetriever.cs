using CMZero.API.ServiceAgent;
using CMZero.Web.Models;
using CMZero.Web.Services.Labels.Mappers;

namespace CMZero.Web.Services.Labels
{
    public class LabelCollectionRetriever : ILabelCollectionRetriever
    {
        private readonly ILabelCollectionMapper _labelCollectionMapper;

        private readonly IContentAreasServiceAgent _contentAreasServiceAgent;

        private readonly ISystemSettings _systemSettings;

        public LabelCollectionRetriever(ILabelCollectionMapper labelCollectionMapper, IContentAreasServiceAgent contentAreasServiceAgent, ISystemSettings systemSettings)
        {
            _labelCollectionMapper = labelCollectionMapper;
            _contentAreasServiceAgent = contentAreasServiceAgent;
            _systemSettings = systemSettings;
        }

        public LabelCollection Get(string collectionName)
        {
            var apiKey = _systemSettings.ApiKey;

            return _labelCollectionMapper.Map(_contentAreasServiceAgent.GetByCollectionNameAndApiKey(apiKey, collectionName));
        }
    }
}