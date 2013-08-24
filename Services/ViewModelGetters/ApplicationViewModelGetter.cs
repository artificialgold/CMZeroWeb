using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.Web.Models;
using CMZero.Web.Models.ViewModels;
using CMZero.Web.Services.Applications;
using CMZero.Web.Services.Labels;

namespace CMZero.Web.Services.ViewModelGetters
{
    public class ApplicationViewModelGetter : IApplicationViewModelGetter
    {
        private readonly IApplicationService _applicationService;
        private readonly ILabelCollectionRetriever _labelCollectionRetriever;

        public ApplicationViewModelGetter(IApplicationService applicationService, ILabelCollectionRetriever labelCollectionRetriever)
        {
            _applicationService = applicationService;
            _labelCollectionRetriever = labelCollectionRetriever;
        }

        public ApplicationViewModel Get(string applicationId)
        {
            var application = _applicationService.GetById(applicationId);

            var model = new ApplicationViewModel();

            return ConstructViewModel(model,application);
        }

        private ApplicationViewModel ConstructViewModel(ApplicationViewModel model, Application application)
        {
            var labels = GetLabelCollection();

            model.Application = application;
            model.Labels = labels;

            return model;
        }

        private LabelCollection GetLabelCollection()
        {
            return _labelCollectionRetriever.Get("ApplicationPage");
        }

        public ApplicationViewModel Update(string applicationId, string name, bool active)
        {
            //Get labels here and build the success message (remove the ConstructViewModel part)
            var labels = GetLabelCollection();

            var model = new ApplicationViewModel
                {
                    Labels = labels,
                    Success = true,
                    SuccessMessageType = SuccessMessageTypes.Success,
                };

            model.SuccessMessage = model.GetLabel("UpdateSuccessMessage");

            try
            {
                model.Application = _applicationService.Update(applicationId, name, active);
            }
            catch (ApplicationNameAlreadyExistsException)
            {
                model.Application = _applicationService.GetById(applicationId);
                model.Success = false;
                model.SuccessMessageType = SuccessMessageTypes.ApplicationNameAlreadyExists;
                model.FailureMessage = model.GetLabel("UpdateFailureMessageApplicationNameExists");
            }

            return ConstructViewModel(model, model.Application);
        }
    }
}