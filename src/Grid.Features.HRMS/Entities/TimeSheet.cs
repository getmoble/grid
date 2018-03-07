using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grid.Features.HRMS.Entities
{
    public class TimeSheet : UserCreatedEntityBase
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public double TotalHours { get; set; }
        public string Comments { get; set; }
        public TimeSheetState State { get; set; }

        [NotMapped]
        public string StateStyle
        {
            get
            {
                switch (State)
                {
                    case TimeSheetState.PendingApproval:
                        return "timesheet-pending";
                    case TimeSheetState.NeedsCorrection:
                        return "timesheet-needscorrection";
                    case TimeSheetState.Approved:
                        return "timesheet-approved";
                    default:
                        return "timesheet-pending";
                }
            }
        }

        public int? ApprovedByUserId { get; set; }
        [ForeignKey("ApprovedByUserId")]
        public User ApprovedByUser { get; set; }
        public string ApproverComments { get; set; }
    }
}