using System.Collections.Generic;
using System.Linq;

using CMZero.API.Messages;
using CMZero.Web.Models;

namespace CMZero.Web.Services.Labels.Mappers
{
    public class LabelCollectionMapper: ILabelCollectionMapper
    {
        private readonly IContentAreaMapper _contentAreaMapper;

        public LabelCollectionMapper(IContentAreaMapper contentAreaMapper)
        {
            _contentAreaMapper = contentAreaMapper;
        }

        public LabelCollection Map(IEnumerable<ContentArea> contentAreas)
        {
            IList<ContentAreaForDisplay> contentAreasForDisplay = (from contentArea in contentAreas where contentArea != null select _contentAreaMapper.Map(contentArea)).ToList();

            return new LabelCollection{ContentAreas = contentAreasForDisplay};
        }
    }
}