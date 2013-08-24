using CMZero.Web.Models.ViewModels;

namespace CMZero.Web.Services.ViewModelGetters
{
    public interface IApplicationViewModelGetter
    {
        ApplicationViewModel Get(string applicationId);

        ApplicationViewModel Update(string applicationId, string name, bool active);
    }
}