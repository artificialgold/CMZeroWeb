using CMZero.Web.Models.ViewModels;

namespace CMZero.Web.Services.ViewModelGetters
{
    public interface ICollectionViewModelGetter
    {
        CollectionViewModel Get(string collectionId);
    }
}