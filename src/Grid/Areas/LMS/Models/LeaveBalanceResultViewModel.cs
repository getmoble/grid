using Grid.Features.Common;


namespace Grid.Areas.LMS.Models
{
    public class LeaveBalanceResultViewModel: ViewModelBase
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public float RequestedDays { get; set; }
        public float CurrentLeaveBalance { get; set; }
    }
}