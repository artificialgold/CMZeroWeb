using System.Collections.Generic;
using CMZero.API.ServiceAgent;
using CMZero.Web.Models;
using CMZero.Web.Models.Logging;
using CMZero.Web.Services.Labels.Mappers;
using CMZero.Web.Services.Logging;

namespace CMZero.Web.Services.Labels
{
    public class LabelCollectionRetriever : ILabelCollectionRetriever
    {
        private readonly ILabelCollectionMapper _labelCollectionMapper;

        private readonly IContentAreasServiceAgent _contentAreasServiceAgent;

        private readonly ISystemSettings _systemSettings;
        private readonly ILogger _logger;

        public LabelCollectionRetriever(ILabelCollectionMapper labelCollectionMapper, IContentAreasServiceAgent contentAreasServiceAgent, ISystemSettings systemSettings, ILogger logger)
        {
            _labelCollectionMapper = labelCollectionMapper;
            _contentAreasServiceAgent = contentAreasServiceAgent;
            _systemSettings = systemSettings;
            _logger = logger;
        }

        public LabelCollection Get(string collectionName)
        {

            var apiKey = _systemSettings.ApiKey;

            //Implement Caching of at least this and possibly more
            try
            {
                return
                    _labelCollectionMapper.Map(_contentAreasServiceAgent.GetByCollectionNameAndApiKey(apiKey,
                                                                                                      collectionName));
            }
            catch
            {
                _logger.LogEvent(new LogEvent("Problem getting collection from Label Collection Retriever"));
                return new LabelCollection{ContentAreas = new List<ContentAreaForDisplay>()};
            }
        }
    }
}