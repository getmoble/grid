using System.ComponentModel.DataAnnotations;
using Grid.Features.Common;

namespace Grid.Features.LMS.ViewModels
{
    public class ApproveRejectLeaveViewModel: ViewModelBase
    {
        [DataType(DataType.MultilineText)]
        [Required]
        public string ApproverComments { get; set; }
    }
}