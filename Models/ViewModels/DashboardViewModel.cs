using System.Collections.Generic;
using CMZero.API.Messages;

namespace CMZero.Web.Models.ViewModels
{
    public class DashboardViewModel : BaseLabelsViewModel
    {
        public Organisation Organisation { get; set; }
        public IEnumerable<Application> Applications { get; set; }
    }
}