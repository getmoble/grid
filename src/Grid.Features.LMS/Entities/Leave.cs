using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Grid.Features.LMS.Entities.Enums;
using Grid.Features.HRMS;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.LMS.Entities
{
    public class Leave : UserCreatedEntityBase
    {
        [Required]
        [UIHint("Date")]
        [DisplayName("Start Date")]
        public DateTime Start { get; set; }

        [Required]
        [UIHint("Date")]
        [DisplayName("End Date")]
        public DateTime End { get; set; }

        [Required]
        [DisplayName("Leave Type")]
        public int LeaveTypeId { get; set; }
        [ForeignKey("LeaveTypeId")]
        public virtual LeaveType LeaveType { get; set; }

        public LeaveDuration Duration { get; set; }

        [DisplayName("Reason")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Reason { get; set; }

        [DisplayName("Status")]
        public LeaveStatus Status { get; set; }


        public int? RequestedForUserId { get; set; }
        [ForeignKey("RequestedForUserId")]
        public Employee RequestedForUser { get; set; }

    
        [NotMapped]
        public string StatusStyle
        {
            get
            {
                switch (Status)
                {
                    case LeaveStatus.Pending:
                        return "timesheet-pending";
                    case LeaveStatus.Rejected:
                        return "timesheet-needscorrection";
                    case LeaveStatus.Approved:
                        return "timesheet-approved";
                    default:
                        return "timesheet-pending";
                }
            }
        }

        public float Count { get; set; }

        public bool IsLOP { get; set; }
        public string CalculationLog { get; set; }

        public int? ApproverId { get; set; }
        [ForeignKey("ApproverId")]
        public Employee Approver { get; set; }
       
        [DisplayName("Approver Comments")]
        [DataType(DataType.MultilineText)]
        public string ApproverComments { get; set; }

        public DateTime? ActedOn { get; set; }
    }
}