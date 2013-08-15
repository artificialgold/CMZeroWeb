using CMZero.API.Messages;

namespace CMZero.Web.Models.ViewModels
{
    public class ApplicationViewModel : BaseLabelsViewModel
    {
        public bool? Success { get; set; }
        public SuccessMessages SuccessMessage { get; set; }
        public Application Application { get; set; }
    }

    public enum SuccessMessages
    {
        Success,
        ApplicationNameAlreadyExists
    }
}