using CMZero.API.ServiceAgent;
using CMZero.Web.Models;
using CMZero.Web.Services.Labels.Mappers;

namespace CMZero.Web.Services.Labels
{
    public class LabelCollectionRetriever
    {
        private readonly ILabelCollectionMapper labelCollectionMapper;

        private readonly IContentAreasServiceAgent contentAreasServiceAgent;

        public LabelCollectionRetriever(ILabelCollectionMapper labelCollectionMapper, IContentAreasServiceAgent contentAreasServiceAgent)
        {
            this.labelCollectionMapper = labelCollectionMapper;
            this.contentAreasServiceAgent = contentAreasServiceAgent;
        }

        public LabelCollection Get(string collectionId)
        {
            return labelCollectionMapper.Map(contentAreasServiceAgent.GetByCollection(collectionId));
        }
    }
}