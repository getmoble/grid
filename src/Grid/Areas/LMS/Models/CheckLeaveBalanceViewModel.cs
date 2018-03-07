using System;
using Grid.Features.LMS.Entities.Enums;
using Grid.Features.Common;


namespace Grid.Areas.LMS.Models
{
    public class CheckLeaveBalanceViewModel: ViewModelBase
    {
        public int LeaveTypeId { get; set; }
        public LeaveDuration Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public float LeaveCount { get; set; }
        public int? RequestedForUserId { get; set; }
    }
}