using System.Collections.Generic;
using CMZero.API.Messages;

namespace CMZero.Web.Models.ViewModels
{
    public class CollectionViewModel : BaseLabelsViewModel
    {
        public bool? Success { get; set; }
 //       public SuccessMessageTypes SuccessMessageType { get; set; }
        public string SuccessMessage { get; set; }
        public Collection Collection { get; set; }
        public string FailureMessage { get; set; }
    }
}