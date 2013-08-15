using System;
using CMZero.API.Messages.Exceptions.Applications;
using CMZero.Web.Models.Exceptions;
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

            var labels = _labelCollectionRetriever.Get("ApplicationPage");

            return new ApplicationViewModel
                {
                    Application = application,
                    Labels = labels
                };
        }

        public ApplicationViewModel Update(string applicationId, string name)
        {
            var model = new ApplicationViewModel
                {
                    Success = true,
                    SuccessMessage = SuccessMessages.Success
                };

            try
            {
                model.Application = _applicationService.Update(applicationId, name);
            }
            catch (ApplicationNameAlreadyExistsException)
            {
                model.Application = _applicationService.GetById(applicationId);
                model.Success = false;
                model.SuccessMessage = SuccessMessages.ApplicationNameAlreadyExists;
                return model;
            }

            return model;
        }
    }
}