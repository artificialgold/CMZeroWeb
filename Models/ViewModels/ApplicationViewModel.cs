using CMZero.API.Messages;

namespace CMZero.Web.Models.ViewModels
{
    public class ApplicationViewModel : BaseLabelsViewModel
    {
        public bool? Success { get; set; }
        public SuccessMessageTypes SuccessMessageType { get; set; }
        public string SuccessMessage { get; set; }
        public Application Application { get; set; }
        public string FailureMessage { get; set; }
    }

    public enum SuccessMessageTypes
    {
        Success,
        ApplicationNameAlreadyExists
    }
}