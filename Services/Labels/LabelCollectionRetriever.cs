using System;

using CMZero.API.ServiceAgent;
using CMZero.Web.Models;
using CMZero.Web.Services.Labels.Mappers;

namespace CMZero.Web.Services.Labels
{
    public class LabelCollectionRetriever
    {
        private readonly ILabelCollectionMapper labelCollectionMapper;

        public LabelCollectionRetriever(ILabelCollectionMapper labelCollectionMapper, IContentAreasServiceAgent contentAreasServiceAgent)
        {
            this.labelCollectionMapper = labelCollectionMapper;
        }

        public LabelCollection Get(string collectionId)
        {
            throw new NotImplementedException();
        }
    }
}