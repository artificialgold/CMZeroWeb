using CMZero.Web.Models;

namespace CMZero.Web.Services.Labels
{
    public interface ILabelCollectionRetriever
    {
        LabelCollection Get(string collectionId);
    }
}