using System.Collections.Generic;

using CMZero.API.Messages;
using CMZero.Web.Models;

namespace CMZero.Web.Services.Labels.Mappers
{
    public interface ILabelCollectionMapper
    {
        LabelCollection Map(List<ContentArea> contentAreas);
    }
}